using DAL;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ReportService
    {
        public async static Task GenerateReport(IProgress<(string, int)> progress = null)
        {
            var business = new ReportDAL();

            List<ReportItem> items = new List<ReportItem>();
            var allAv = await new JavLibraryDAL().GetAvModelByWhere("");
            var allMatch = await GenerateExistingAVs(allAv);

            if (progress != null)
            {
                progress.Report(("total", allAv.Count));
            }

            Report report = new();
            report.ExtensionModel = new Dictionary<string, int>();
            report.TotalCount = allAv.Count;

            var reportId = business.InsertReport(report);
            report.ReportId = reportId;
            int process = 0;

            foreach (var av in allAv)
            {
                await Task.Run(() =>
                {
                    ProcessReportType(av, allMatch, report, items);
                    process++;

                    if (progress != null)
                    {
                        progress.Report(("current", process));
                    }
                });
            };

            business.BatchInserReportItem(items);
            business.UpdateReport(report);
            business.UpdateReportFinish(reportId);
        }

        private static void ProcessReportType(AvModel av, Dictionary<int, List<MyFileInfo>> existFiles, Report report, List<ReportItem> items)
        {
            int exist = 0;
            double existSize = 0d;

            if (existFiles.ContainsKey(av.Id))
            {
                var file = existFiles[av.Id];

                if (file.Count() > 0)
                {
                    var biggestFile = file.FirstOrDefault(x => x.Length == file.Max(y => y.Length));

                    exist = 1;
                    existSize = biggestFile.Length;

                    report.TotalExist += 1;
                    report.TotalExistSize += biggestFile.Length;

                    var extensionKey = biggestFile.Extension;

                    if (report.ExtensionModel.ContainsKey(extensionKey))
                    {
                        report.ExtensionModel[extensionKey] = report.ExtensionModel[extensionKey] + 1;
                    }
                    else
                    {
                        report.ExtensionModel.Add(extensionKey, 1);
                    }

                    if (biggestFile.Length < (long)1 * 1024 * 1024 * 1024)
                    {
                        report.LessThenOneGiga++;
                    }

                    if (biggestFile.Length >= (long)1 * 1024 * 1024 * 1024 && biggestFile.Length < (long)2 * 1024 * 1024 * 1024)
                    {
                        report.OneGigaToTwo++;
                    }

                    if (biggestFile.Length >= (long)2 * 1024 * 1024 * 1024 && biggestFile.Length < (long)4 * 1024 * 1024 * 1024)
                    {
                        report.TwoGigaToFour++;
                    }

                    if (biggestFile.Length >= (long)4 * 1024 * 1024 * 1024 && biggestFile.Length < (long)6 * 1024 * 1024 * 1024)
                    {
                        report.FourGigaToSix++;
                    }

                    if (biggestFile.Length >= (long)6 * 1024 * 1024 * 1024)
                    {
                        report.GreaterThenSixGiga++;
                    }

                    if (biggestFile.Name.Contains("-C" + biggestFile.Extension))
                    {
                        report.ChineseCount++;
                    }
                }
            }

            foreach (ReportType type in Enum.GetValues(typeof(ReportType)))
            {
                switch (type)
                {
                    case ReportType.Actress:
                        foreach (var itemName in av.InfoObj.Where(x => x.Type == CommonModelType.Actress))
                        {
                            ProcessReportItem(ReportType.Actress, itemName.Name, exist, existSize, report.ReportId, items);
                        }
                        break;
                    case ReportType.Category:
                        foreach (var itemName in av.InfoObj.Where(x => x.Type == CommonModelType.Category))
                        {
                            ProcessReportItem(ReportType.Category, itemName.Name, exist, existSize, report.ReportId, items);
                        }
                        break;
                    case ReportType.Company:
                        foreach (var itemName in av.InfoObj.Where(x => x.Type == CommonModelType.Company))
                        {
                            ProcessReportItem(ReportType.Company, itemName.Name, exist, existSize, report.ReportId, items);
                        }
                        break;
                    case ReportType.Date:
                        ProcessReportItem(ReportType.Date, av.ReleaseDate?.ToString("yyyy"), exist, existSize, report.ReportId, items);
                        break;
                    case ReportType.Director:
                        foreach (var itemName in av.InfoObj.Where(x => x.Type == CommonModelType.Director))
                        {
                            ProcessReportItem(ReportType.Director, itemName.Name, exist, existSize, report.ReportId, items);
                        }
                        break;
                    case ReportType.Prefix:
                        var prefix = av.AvId.Split('-').Length >= 2 ? av.AvId.Split('-')[0] : "";
                        if (!string.IsNullOrEmpty(prefix))
                        {
                            ProcessReportItem(ReportType.Prefix, prefix, exist, existSize, report.ReportId, items);
                        }
                        break;
                    case ReportType.Publisher:
                        foreach (var itemName in av.InfoObj.Where(x => x.Type == CommonModelType.Publisher))
                        {
                            ProcessReportItem(ReportType.Publisher, itemName.Name, exist, existSize, report.ReportId, items);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ProcessReportItem(ReportType type, string itemName, int exist, double existSize, int reportId, List<ReportItem> items)
        {
            var tempItem = items.FirstOrDefault(x => x.ReportId == reportId && x.ItemName == itemName && (ReportType)x.ReportType == type);
            if (tempItem != null)
            {
                tempItem.TotalSize += existSize;
                tempItem.ExistCount += exist;
                tempItem.TotalCount += 1;
            }
            else
            {
                items.Add(new ReportItem()
                {
                    ExistCount = exist,
                    ItemName = itemName,
                    ReportType = (int)type,
                    TotalCount = 1,
                    TotalSize = existSize,
                    ReportId = reportId
                });
            }
        }

        private async static Task<Dictionary<int, List<MyFileInfo>>> GenerateExistingAVs(List<AvModel> avs)
        {
            Dictionary<int, List<MyFileInfo>> fileContainer = new Dictionary<int, List<MyFileInfo>>();
            List<OneOneFiveFileItemModel> oneOneFiveFiles = new();

            oneOneFiveFiles = await OneOneFiveService.Get115AllFilesModel(OneOneFiveFolder.Fin);
            await RedisService.SetHashAndReplaceAsync("115", "allfiles", JsonConvert.SerializeObject(oneOneFiveFiles));

            var dic = avs.GroupBy(x => x.Name.ToUpper()).ToDictionary(x => x.Key, x => x.ToList());

            foreach(var file in oneOneFiveFiles)
            {               
                var key = file.AvName.ToUpper();
                if (dic.ContainsKey(key))
                {
                    var match = dic[key].FirstOrDefault();

                    if (match != null)
                    {
                        if (fileContainer.ContainsKey(match.Id))
                        {
                            fileContainer[match.Id].Add(new MyFileInfo()
                            {
                                Extension = "." + file.ico,
                                FullName = file.n,
                                Length = file.s,
                                Name = file.n
                            });
                        }
                        else
                        {
                            fileContainer.Add(match.Id, new List<MyFileInfo> { 
                                new MyFileInfo() {
                                    Extension = "." + file.ico,
                                    FullName = file.n,
                                    Length = file.s,
                                    Name = file.n
                                }
                            });
                        }
                    }
                }
            }

            return fileContainer;
        }
    }
}
