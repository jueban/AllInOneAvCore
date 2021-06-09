using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utils;

namespace WebMVC.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            var reports = new ReportDAL().GetReports();

            ViewData.Add("reports", reports);
            ViewData.Add("Title", "报表-选择");

            return View();
        }

        public async Task<JsonResult> GenerateReport()
        {
            var setting = await SettingService.GetSetting();
            using (HttpClient hc = new())
            {
                await hc.GetAsync(setting.HangfireSite + "/job/GenerateReport");
            }

            return Json(new { success = true });
        }

        public IActionResult ShowChart(int id)
        {
            ViewData.Add("Title", "报表-详情");

            var business = new ReportDAL();

            var report = business.GetReport(id);
            var items = business.ReportItem(report.ReportId);

            string extension = "['后缀', 'total']";
            Dictionary<string, string> actress = new();
            Dictionary<string, string> category = new();
            Dictionary<string, string> director = new();
            Dictionary<string, string> company = new();
            Dictionary<string, string> publisher =new();
            Dictionary<string, string> prefix = new();
            Dictionary<string, string> date = new();

            Dictionary<string, string> actressRatio = new();
            Dictionary<string, string> categoryRatio = new();
            Dictionary<string, string> directorRatio = new();
            Dictionary<string, string> companyRatio = new();
            Dictionary<string, string> publisherRatio = new();
            Dictionary<string, string> prefixRatio = new();
            Dictionary<string, string> dateRatio = new();

            Dictionary<string, string> actressSize = new();
            Dictionary<string, string> categorySize = new();
            Dictionary<string, string> directorSize = new();
            Dictionary<string, string> companySize = new();
            Dictionary<string, string> publisherSize = new();
            Dictionary<string, string> prefixSize = new();
            Dictionary<string, string> dateSize = new();

            List<string> actressKey = new();
            List<int> actressValue = new();
            List<string> categoryKey = new();
            List<int> categoryValue = new();
            List<string> directorKey = new();
            List<int> directorValue = new();
            List<string> companyKey = new();
            List<int> companyValue = new();
            List<string> publisherKey = new();
            List<int> publisherValue = new();
            List<string> prefixKey = new();
            List<int> prefixValue = new();
            List<string> dateKey = new();
            List<int> dateValue = new();

            List<string> actressRatioKey = new();
            List<decimal> actressRatioValue = new();
            List<string> categoryRatioKey = new();
            List<decimal> categoryRatioValue = new();
            List<string> directorRatioKey = new();
            List<decimal> directorRatioValue = new();
            List<string> companyRatioKey = new();
            List<decimal> companyRatioValue = new();
            List<string> publisherRatioKey = new();
            List<decimal> publisherRatioValue = new();
            List<string> prefixRatioKey = new();
            List<decimal> prefixRatioValue = new();
            List<string> dateRatioKey = new();
            List<decimal> dateRatioValue = new();

            List<string> actressSizeKey = new();
            List<double> actressSizeValue = new();
            List<string> categorySizeKey = new();
            List<double> categorySizeValue = new();
            List<string> directorSizeKey = new();
            List<double> directorSizeValue = new();
            List<string> companySizeKey = new();
            List<double> companySizeValue = new();
            List<string> publisherSizeKey = new();
            List<double> publisherSizeValue = new();
            List<string> prefixSizeKey = new();
            List<double> prefixSizeValue = new();
            List<string> dateSizeKey = new();
            List<double> dateSizeValue = new();

            var extensionModel = JsonConvert.DeserializeObject<Dictionary<string, int>>(report.Extension);

            foreach (var e in extensionModel)
            {
                extension += string.Format(",['{1}',{0}]", e.Value, e.Key);
            }

            foreach (ReportType type in Enum.GetValues(typeof(ReportType)))
            {
                List<ReportItem> i = new List<ReportItem>();
                switch (type)
                {
                    case ReportType.Actress:
                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            actressKey.Add("'" + temp.ItemName + "'");
                            actressValue.Add(temp.ExistCount);
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type && x.TotalCount >= 100).OrderByDescending(x => (decimal)x.ExistCount / (decimal)x.TotalCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            actressRatioKey.Add("'" + temp.ItemName + "'");
                            actressRatioValue.Add(Math.Round(((decimal)temp.ExistCount / (decimal)temp.TotalCount) * 100, 1));
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.TotalSize).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            actressSizeKey.Add("'" + temp.ItemName + "'");
                            actressSizeValue.Add(temp.TotalSize);
                        }

                        break;
                    case ReportType.Category:
                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            categoryKey.Add("'" + temp.ItemName + "'");
                            categoryValue.Add(temp.ExistCount);
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type && x.TotalCount >= 100).OrderByDescending(x => (decimal)x.ExistCount / (decimal)x.TotalCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            categoryRatioKey.Add("'" + temp.ItemName + "'");
                            categoryRatioValue.Add(Math.Round(((decimal)temp.ExistCount / (decimal)temp.TotalCount) * 100, 1));
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.TotalSize).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            categorySizeKey.Add("'" + temp.ItemName + "'");
                            categorySizeValue.Add(temp.TotalSize);
                        }

                        break;
                    case ReportType.Director:
                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            directorKey.Add("'" + temp.ItemName + "'");
                            directorValue.Add(temp.ExistCount);
                        }


                        i = items.Where(x => (ReportType)x.ReportType == type && x.TotalCount >= 100).OrderByDescending(x => (decimal)x.ExistCount / (decimal)x.TotalCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            directorRatioKey.Add("'" + temp.ItemName + "'");
                            directorRatioValue.Add(Math.Round(((decimal)temp.ExistCount / (decimal)temp.TotalCount) * 100, 1));
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.TotalSize).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            directorSizeKey.Add("'" + temp.ItemName + "'");
                            directorSizeValue.Add(temp.TotalSize);
                        }

                        break;
                    case ReportType.Company:
                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            companyKey.Add("'" + temp.ItemName + "'");
                            companyValue.Add(temp.ExistCount);
                        }


                        i = items.Where(x => (ReportType)x.ReportType == type && x.TotalCount >= 100).OrderByDescending(x => (decimal)x.ExistCount / (decimal)x.TotalCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            companyRatioKey.Add("'" + temp.ItemName + "'");
                            companyRatioValue.Add(Math.Round(((decimal)temp.ExistCount / (decimal)temp.TotalCount) * 100, 1));
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.TotalSize).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            companySizeKey.Add("'" + temp.ItemName + "'");
                            companySizeValue.Add(temp.TotalSize);
                        }

                        break;
                    case ReportType.Publisher:
                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            publisherKey.Add("'" + temp.ItemName + "'");
                            publisherValue.Add(temp.ExistCount);
                        }


                        i = items.Where(x => (ReportType)x.ReportType == type && x.TotalCount >= 100).OrderByDescending(x => (decimal)x.ExistCount / (decimal)x.TotalCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            publisherRatioKey.Add("'" + temp.ItemName + "'");
                            publisherRatioValue.Add(Math.Round(((decimal)temp.ExistCount / (decimal)temp.TotalCount) * 100, 1));
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.TotalSize).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            publisherSizeKey.Add("'" + temp.ItemName + "'");
                            publisherSizeValue.Add(temp.TotalSize);
                        }

                        break;
                    case ReportType.Prefix:
                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            prefixKey.Add("'" + temp.ItemName + "'");
                            prefixValue.Add(temp.ExistCount);
                        }


                        i = items.Where(x => (ReportType)x.ReportType == type && x.TotalCount >= 100).OrderByDescending(x => (decimal)x.ExistCount / (decimal)x.TotalCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            prefixRatioKey.Add("'" + temp.ItemName + "'");
                            prefixRatioValue.Add(Math.Round(((decimal)temp.ExistCount / (decimal)temp.TotalCount) * 100, 1));
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.TotalSize).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            prefixSizeKey.Add("'" + temp.ItemName + "'");
                            prefixSizeValue.Add(temp.TotalSize);
                        }

                        break;
                    case ReportType.Date:
                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            dateKey.Add("'" + temp.ItemName + "'");
                            dateValue.Add(temp.ExistCount);
                        }


                        i = items.Where(x => (ReportType)x.ReportType == type && x.TotalCount >= 100).OrderByDescending(x => (decimal)x.ExistCount / (decimal)x.TotalCount).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            dateRatioKey.Add("'" + temp.ItemName + "'");
                            dateRatioValue.Add(Math.Round(((decimal)temp.ExistCount / (decimal)temp.TotalCount) * 100, 1));
                        }

                        i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.TotalSize).Take(20).ToList();

                        foreach (var temp in i)
                        {
                            dateSizeKey.Add("'" + temp.ItemName + "'");
                            dateSizeValue.Add(temp.TotalSize);
                        }

                        break;
                }
            }

            actress.Add("[" + string.Join(",", actressKey) + "]", "[" + string.Join(",", actressValue) + "]");
            category.Add("[" + string.Join(",", categoryKey) + "]", "[" + string.Join(",", categoryValue) + "]");
            director.Add("[" + string.Join(",", directorKey) + "]", "[" + string.Join(",", directorValue) + "]");
            company.Add("[" + string.Join(",", companyKey) + "]", "[" + string.Join(",", companyValue) + "]");
            publisher.Add("[" + string.Join(",", publisherKey) + "]", "[" + string.Join(",", publisherValue) + "]");
            date.Add("[" + string.Join(",", dateKey) + "]", "[" + string.Join(",", dateValue) + "]");
            prefix.Add("[" + string.Join(",", prefixKey) + "]", "[" + string.Join(",", prefixValue) + "]");

            actressRatio.Add("[" + string.Join(",", actressRatioKey) + "]", "[" + string.Join(",", actressRatioValue) + "]");
            categoryRatio.Add("[" + string.Join(",", categoryRatioKey) + "]", "[" + string.Join(",", categoryRatioValue) + "]");
            directorRatio.Add("[" + string.Join(",", directorRatioKey) + "]", "[" + string.Join(",", directorRatioValue) + "]");
            companyRatio.Add("[" + string.Join(",", companyRatioKey) + "]", "[" + string.Join(",", companyRatioValue) + "]");
            publisherRatio.Add("[" + string.Join(",", publisherRatioKey) + "]", "[" + string.Join(",", publisherRatioValue) + "]");
            dateRatio.Add("[" + string.Join(",", dateRatioKey) + "]", "[" + string.Join(",", dateRatioValue) + "]");
            prefixRatio.Add("[" + string.Join(",", prefixRatioKey) + "]", "[" + string.Join(",", prefixRatioValue) + "]");

            actressSize.Add("[" + string.Join(",", actressSizeKey) + "]", "[" + string.Join(",", actressSizeValue) + "]");
            categorySize.Add("[" + string.Join(",", categorySizeKey) + "]", "[" + string.Join(",", categorySizeValue) + "]");
            directorSize.Add("[" + string.Join(",", directorSizeKey) + "]", "[" + string.Join(",", directorSizeValue) + "]");
            companySize.Add("[" + string.Join(",", companySizeKey) + "]", "[" + string.Join(",", companySizeValue) + "]");
            publisherSize.Add("[" + string.Join(",", publisherSizeKey) + "]", "[" + string.Join(",", publisherSizeValue) + "]");
            dateSize.Add("[" + string.Join(",", dateSizeKey) + "]", "[" + string.Join(",", dateSizeValue) + "]");
            prefixSize.Add("[" + string.Join(",", prefixSizeKey) + "]", "[" + string.Join(",", prefixSizeValue) + "]");

            ViewData.Add("countString", string.Format("['总数', 'total'],['总共',{0}],['存在',{1}]", report.TotalCount, report.TotalExist));
            ViewData.Add("chineseString", string.Format("['中文', 'total'],['总共',{0}],['中文',{1}]", report.TotalCount, report.ChineseCount));
            ViewData.Add("sizeString", string.Format("['大小', 'total'],['[0,1GB)',{0}], ['[1GB,2GB)',{1}], ['[2GB,4GB)',{2}], ['[4GB,6GB)',{3}], ['[6GB,∞)',{4}]", report.LessThenOneGiga, report.OneGigaToTwo, report.TwoGigaToFour, report.FourGigaToSix, report.GreaterThenSixGiga));
            ViewData.Add("extensionString", extension);

            ViewData.Add("actressString", actress);
            ViewData.Add("categoryString", category);
            ViewData.Add("directorString", director);
            ViewData.Add("companyString", company);
            ViewData.Add("publisherString", publisher);
            ViewData.Add("dateString", date);
            ViewData.Add("prefixString", prefix);

            ViewData.Add("actressRatioString", actressRatio);
            ViewData.Add("categoryRatioString", categoryRatio);
            ViewData.Add("directorRatioString", directorRatio);
            ViewData.Add("companyRatioString", companyRatio);
            ViewData.Add("publisherRatioString", publisherRatio);
            ViewData.Add("dateRatioString", dateRatio);
            ViewData.Add("prefixRatioString", prefixRatio);

            ViewData.Add("actressSizeString", actressSize);
            ViewData.Add("categorySizeString", categorySize);
            ViewData.Add("directorSizeString", directorSize);
            ViewData.Add("companySizeString", companySize);
            ViewData.Add("publisherSizeString", publisherSize);
            ViewData.Add("dateSizeString", dateSize);
            ViewData.Add("prefixSizeString", prefixSize);

            ViewData.Add("count", report.TotalExist);
            ViewData.Add("size", FileUtility.GetAutoSizeString((long)report.TotalExistSize, 1));

            return View();
        }
    }
}
