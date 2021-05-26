using DAL;
using HtmlAgilityPack;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class JavbusService
    {
        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36";
        private static readonly string JavBusIndexUrl = "https://www.javbus.com/";

        public static (CookieContainer, string) GetJavBusCookie()
        {
            return new(new CookieContainer(), DefaultUserAgent);
        }

        public async static Task<int> SaveCommonJavBusModel(List<CommonModel> list)
        {
            int ret = 0;

            foreach (var l in list)
            {
                ret += await new JavBusDAL().InsertCommonJavBusModel(l);
            }

            return ret;
        }

        public async static Task<int> SaveJavBusAvModel(AvModel model)
        {
            var ret = await new JavBusDAL().InsertAvModel(model);

            return ret;
        }

        public async static Task<List<CommonModel>> GetJavBusCategory()
        {
            List<CommonModel> ret = new List<CommonModel>();

            var res = await GetJavBusContent(GetJavLibraryEntryUrl(JavBusEntryPointType.Category, "", 1));

            if (res.exception == null && !string.IsNullOrEmpty(res.content))
            {
                HtmlDocument htmlDocument = new();
                htmlDocument.LoadHtml(res.content);

                var genreListPath = "//div[@class='row genre-box']";

                var genreListNodes = htmlDocument.DocumentNode.SelectNodes(genreListPath);

                foreach (var node in genreListNodes)
                {
                    foreach (var subNode in node.ChildNodes.Where(x => x.Name == "a"))
                    {
                        var aTagHref = subNode.Attributes["href"].Value.Trim();
                        var aTagTitle = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(subNode.InnerText.Trim()));

                        ret.Add(new CommonModel()
                        {
                            Name = aTagTitle,
                            Type = CommonModelType.Category,
                            Url = aTagHref
                        });
                    }
                }
            }

            return ret;
        }

        public async static Task<(List<CommonModel> actress, List<(string url, string file)> pics)> GetJavBusActress()
        {
            (List<CommonModel> actress, List<(string url, string file)> pics) ret = new();

            List<CommonModel> actress = new ();
            List<(string, string)> imgs = new();
            bool lastPage = false;
            int index = 1;

            while (!lastPage)
            {
                var res = await GetJavBusContent(GetJavLibraryEntryUrl(JavBusEntryPointType.Actress, "", index));

                lastPage = res.lastPage;

                if (!lastPage)
                {
                    Console.WriteLine($"正在获取女优列表第{index}页，不是最后一页");

                    index++;

                    if (res.exception == null && !string.IsNullOrEmpty(res.content))
                    {

                        HtmlDocument htmlDocument = new();
                        htmlDocument.LoadHtml(res.content);

                        var actressListPath = "//a[@class='avatar-box text-center']";

                        var actressListNodes = htmlDocument.DocumentNode.SelectNodes(actressListPath);

                        foreach (var node in actressListNodes)
                        {
                            var aTagHref = node.Attributes["href"].Value.Trim();
                            var imgNode = node.SelectSingleNode(".//img");
                            var aTagTitle = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(imgNode.Attributes["title"].Value.Trim()));
                            var img = imgNode.Attributes["src"].Value.Trim();

                            Console.WriteLine($"正在获取女优{aTagTitle}");

                            var imageFolder = SettingService.GetSetting().Result.AvatorImageFolder;
                            var picFile = imageFolder + aTagTitle + ".jpg";

                            imgs.Add((img, picFile));

                            actress.Add(new CommonModel()
                            {
                                Name = aTagTitle,
                                Type = CommonModelType.Actress,
                                Url = aTagHref
                            });
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{index}是最后一页");
                }
            }

            ret.actress = actress;
            ret.pics = imgs;

            return ret;
        }

        public async static Task<(List<WebScanUrlModel> success, List<string> fails)> GetJavBusList(JavBusEntryPointType entry, string url, int page, IProgress<string> progress, bool magOnly = false)
        {
            (List<WebScanUrlModel> success, List<string> fails) ret = new();
            List<WebScanUrlModel> retList = new();
            List<string> failList = new();
            var lastPage = false;
            int currentIndex = 1;

            var realUlr = GetJavLibraryEntryUrl(entry, url, currentIndex);
            var res = await GetJavBusContent(realUlr, magOnly);

            lastPage = res.lastPage;

            while (res.exception == null && !lastPage && currentIndex <= page && !string.IsNullOrEmpty(res.content))
            {
                if (res.exception == null && !string.IsNullOrEmpty(res.content))
                {
                    progress.Report($"获取 {url} 的第{currentIndex}页 成功");

                    HtmlDocument document = new();
                    document.LoadHtml(res.content);

                    var avListPath = "//a[@class='movie-box']";
                    var avIdPath = ".//date";
                    var avImgPath = ".//img";

                    var avListNodes = document.DocumentNode.SelectNodes(avListPath);

                    progress.Report($"{url} 第 {currentIndex} 页共有 {avListNodes.Count} 个AV");

                    foreach (var av in avListNodes)
                    {
                        try
                        {
                            WebScanUrlModel temp = new WebScanUrlModel
                            {
                                URL = av.Attributes["href"].Value.Trim(),
                                AvId = av.SelectNodes(avIdPath).First().InnerText.Trim(),
                                Name = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(av.SelectSingleNode(avImgPath).Attributes["title"].Value.Trim())),
                                IsDownload = false,
                                ScanUrlSite = WebScanUrlSite.JavBus
                            };

                            retList.Add(temp);
                        }
                        catch (Exception)
                        {
                            failList.Add(realUlr);
                        }
                    }
                }
                else
                {
                    failList.Add(realUlr);
                }

                realUlr = GetJavLibraryEntryUrl(entry, url, ++currentIndex);
                res = await GetJavBusContent(realUlr);
            }

            ret.success = retList;
            ret.fails = failList;
            return ret;
        }

        public async static Task<(AvModel avModel, List<SeedMagnetSearchModel> mags)> GetJavBusDetail(string url, bool getMag = false)
        {
            (AvModel avModel, List<SeedMagnetSearchModel> mags) ret = new();
            List<SeedMagnetSearchModel> magList = new List<SeedMagnetSearchModel>();
            ret.mags = magList;
            AvModel avModel = new();

            var imageFolder = SettingService.GetSetting().Result.JavBusImageFolder;

            if (!Directory.Exists(imageFolder))
            {
                Directory.CreateDirectory(imageFolder);
            }

            var res = await GetJavBusContent(url);

            if (res.exception == null && !string.IsNullOrEmpty(res.content))
            {
                avModel = GetJavBusAvModel(res.content);
                avModel.Url = url;

                await SaveJavBusAvModel(avModel);
                await SaveCommonJavBusModel(avModel.InfoObj);

                var picFile = imageFolder + "\\" + avModel.FileNameWithoutExtension + ".jpg";

                if (!string.IsNullOrWhiteSpace(avModel.PicUrl) && !File.Exists(picFile))
                {
                    try
                    {
                        new WebClient().DownloadFile(avModel.PicUrl, picFile);
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }

            ret.avModel = avModel;

            return ret;
        }

        public async static Task<string> GetListPageName(string url)
        {
            string ret = "";

            var res = await GetJavBusContent(url, false);

            if(res.exception == null && !string.IsNullOrEmpty(res.content))
            {
                HtmlDocument document = new();
                document.LoadHtml(res.content);

                var namePath = "//div[@class='alert alert-success alert-common']//b";

                var nameNode = document.DocumentNode.SelectSingleNode(namePath);

                if (nameNode != null)
                {
                    ret = nameNode.InnerText.Split('-')[0].Trim().ToUpper();
                }
            }

            return ret;
        }

        #region 内部使用
        //获取JavLibrary各个入口的Url
        private static string GetJavLibraryEntryUrl(JavBusEntryPointType type, string content, int page)
        {
            string ret = "";

            switch (type)
            {
                case JavBusEntryPointType.HomePage:
                    ret = JavBusIndexUrl + "page/" + page;
                    break;

                case JavBusEntryPointType.Search:
                    ret = JavBusIndexUrl + content + "/" + page;
                    break;

                case JavBusEntryPointType.Category:
                    ret = JavBusIndexUrl + "genre";
                    break;

                case JavBusEntryPointType.Actress:
                    ret = JavBusIndexUrl + "actresses/" + content + "/" + page;
                    break;

                case JavBusEntryPointType.Detail:
                    ret = JavBusIndexUrl + content;
                    break;

                case JavBusEntryPointType.Passin:
                    ret = content + "/" + page;
                    break;

                case JavBusEntryPointType.Director:
                    ret = JavBusIndexUrl + "director/" + content + "/" + page;
                    break;

                case JavBusEntryPointType.Company:
                    ret = JavBusIndexUrl + "studio/" + content + "/" + page;
                    break;

                case JavBusEntryPointType.Publisher:
                    ret = JavBusIndexUrl + "label/" + content + "/" + page;
                    break;
            }

            return ret;
        }

        //通用获取JavLibrary的网页内容
        private async static Task<(Exception exception, string content, bool lastPage)> GetJavBusContent(string url, bool magOnly = false)
        {
            (Exception, string, bool) ret = new();
            var cookie = GetJavBusCookie();
            Exception exception = null;
            string content = "";

            if (cookie.Item1 != null && !string.IsNullOrEmpty(cookie.Item2))
            {
                CookieContainer cc = cookie.Item1;
                var ccStr = "";

                if (cc.Count > 0)
                {
                    ccStr = cc.GetCookieHeader(new Uri(JavBusIndexUrl));
                }

                if (magOnly)
                {
                    ccStr += "existmag=mag";
                }
                else
                {
                    ccStr += "existmag=all";
                }

                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Add("Cookie", ccStr);
                    client.DefaultRequestHeaders.Add("User-Agent", cookie.Item2);
                    try
                    {
                        content = await client.GetStringAsync(url);
                    }
                    catch (HttpRequestException ee)
                    {
                        ret.Item3 = ee.StatusCode == HttpStatusCode.NotFound;
                    }
                    catch (Exception ee)
                    {
                        exception = ee;
                    }
                }
            }

            ret.Item1 = exception;
            ret.Item2 = content;

            return ret;
        }

        private static AvModel GetJavBusAvModel(string content)
        { 
            AvModel ret = new AvModel();
            List<CommonModel> infos = new();
            HtmlDocument document = new();
            document.LoadHtml(content);

            var avBlockPath = "//div[@class='container']";
            var avNamePath = ".//a[@class='bigImage']//img";
            var avInfoPath = ".//span[@class='header']";
            var categoryPath = ".//span[@class='genre']//label//a";
            var actressPath = ".//div[@class='star-name']//a";

            try
            {
                var avBlockNode = document.DocumentNode.SelectSingleNode(avBlockPath);
                var infoNodes = avBlockNode.SelectNodes(avInfoPath);
                var categoryNodes = avBlockNode.SelectNodes(categoryPath);
                var actressNodes = avBlockNode.SelectNodes(actressPath);

                ret.Name = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(avBlockNode.SelectSingleNode(avNamePath).Attributes["title"].Value.Trim()));
                ret.PicUrl = avBlockNode.SelectSingleNode(avNamePath).Attributes["src"].Value.Trim();

                if (infoNodes != null)
                {
                    foreach (var infoNode in infoNodes)
                    {
                        switch (infoNode.InnerText)
                        {
                            case "識別碼:":
                                ret.AvId = infoNode.ParentNode.ChildNodes[2].InnerText.Trim();
                                break;
                            case "發行日期:":
                                ret.ReleaseDate = DateTime.Parse(infoNode.ParentNode.ChildNodes[1].InnerText.Trim());
                                break;
                            case "長度:":
                                ret.AvLength = int.Parse(infoNode.ParentNode.ChildNodes[1].InnerText.Trim().Replace("分鐘", ""));
                                break;
                            case "導演:":
                                infos.Add(new CommonModel
                                {
                                    Name = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(infoNode.ParentNode.ChildNodes[2].InnerText.Trim())),
                                    Type = CommonModelType.Director,
                                    Url = infoNode.ParentNode.ChildNodes[2].Attributes["href"].Value.Trim()
                                });
                                break;
                            case "製作商:":
                                infos.Add(new CommonModel
                                {
                                    Name = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(infoNode.ParentNode.ChildNodes[2].InnerText.Trim())),
                                    Type = CommonModelType.Company,
                                    Url = infoNode.ParentNode.ChildNodes[2].Attributes["href"].Value.Trim()
                                });
                                break;
                            case "發行商:":
                                infos.Add(new CommonModel
                                {
                                    Name = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(infoNode.ParentNode.ChildNodes[2].InnerText.Trim())),
                                    Type = CommonModelType.Publisher,
                                    Url = infoNode.ParentNode.ChildNodes[2].Attributes["href"].Value.Trim()
                                });
                                break;
                            case "系列:":
                                infos.Add(new CommonModel
                                {
                                    Name = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(infoNode.ParentNode.ChildNodes[2].InnerText.Trim())),
                                    Type = CommonModelType.Series,
                                    Url = infoNode.ParentNode.ChildNodes[2].Attributes["href"].Value.Trim()
                                });
                                break;
                        }
                    }
                }

                if (categoryNodes != null)
                {
                    foreach (var category in categoryNodes)
                    {
                        infos.Add(new CommonModel
                        {
                            Name = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(category.InnerText.Trim())),
                            Type = CommonModelType.Category,
                            Url = category.Attributes["href"].Value.Trim()
                        });
                    }
                }

                if (actressNodes != null)
                {
                    foreach (var actress in actressNodes)
                    {
                        infos.Add(new CommonModel
                        {
                            Name = FileUtility.ReplaceInvalidChar(FileUtility.FanToJian(actress.InnerText.Trim())),
                            Type = CommonModelType.Actress,
                            Url = actress.Attributes["href"].Value.Trim()
                        });
                    }
                }

                ret.FileNameWithoutExtension = ret.AvId + "-" + ret.Name;
                ret.Site = WebScanUrlSite.JavBus;
            }
            catch (Exception)
            {

            }

            ret.Infos = JsonHelper.SerializeWithUtf8(infos);

            return ret;
        }
        #endregion
    }
}