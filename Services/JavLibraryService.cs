using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Models;
using Polly;
using Utils;

namespace Services
{
    public class JavLibraryService
    {
        #region 全局变量
        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36";
        private static readonly string JavLibraryCookieDomain = ".javlibrary.com";
        private static readonly string JavLibraryIndexUrl = "http://www.javlibrary.com/cn/";
        private static readonly string JavLibraryCategoryUrl = "http://www.javlibrary.com/cn/genres.php";
        #endregion

        #region Cookie
        //获取JavLibraryCookie
        public async static Task<(CookieContainer, string)> GetJavLibraryCookie()
        {
            string userAgent = "";
            CookieContainer ret = null;

            var mode = SettingService.GetSetting().Result.JavLibrarySettings.CookieMode;
            
            if(mode == JavLibraryGetCookieMode.MockBroswer)
            {
                //TODO 如果有需要JavLibraryCookie的接口调用失败（认为Cookie失效）则删除数据库中Cookie记录，走False分支
                if(new JavLibraryDAL().GetJavLibraryCookie().Result != null)
                {
                    var res = await GetJavChromeCookieFromDB();

                    if(res.cc != null && !string.IsNullOrEmpty(res.userAgent))
                    {
                        ret = res.cc;
                        userAgent = res.userAgent;
                    }
                }
                else
                {
                    var res = await GetJavCookieChromeProcess();

                    if(res.cc != null && !string.IsNullOrEmpty(res.userAgent))
                    {
                        ret = res.cc;
                        userAgent = res.userAgent;
                    }
                }
            }

            if (mode == JavLibraryGetCookieMode.Easy)
            {
                ret = new CookieContainer();
                userAgent = DefaultUserAgent;
            }

            return new (ret, userAgent);
        }

        //读取从油猴脚本传过来的Cookie再结合上从本地Chrome读取到的HttpOnly的Cookie
        public async static Task<int> SaveJavLibraryCookie(string cookie, string userAgent)
        {
            List<CookieItem> items = new();        
            int res = 0;

            if (!string.IsNullOrEmpty(cookie))
            {
                foreach (var item in cookie.Split(';'))
                {
                    items.Add(new CookieItem()
                    {
                        Name = item.Split('=')[0].Trim(),
                        Value = item.Split('=')[1].Trim(),
                    });
                }

                var httpOnlyCookie = CookieService.ReadChromeCookie(JavLibraryCookieDomain);

                if (httpOnlyCookie != null)
                {
                    items.AddRange(httpOnlyCookie);
                }

                if(items != null && items.Count > 0)
                {
                    var business = new JavLibraryDAL();

                    await business.DeleteJavLibraryCookie();

                    JavLibraryCookieJson entity = new()
                    {
                        CookieJson = JsonSerializer.Serialize(items),
                        UserAgent = userAgent
                    };

                    res = await business.InsertJavLibraryCookie(entity);
                }
            }

            return res;
        }
        #endregion

        #region 网页
        //保存JavLibray的通用格式数据，女优，导演，类型，发行商和公司
        public async static Task<int> SaveCommonJavLibraryModel(List<CommonModel> list)
        {
            int ret = 0;

            foreach(var l in list)
            {
                ret += await new JavLibraryDAL().InsertCommonJavLibraryModel(l);
            }

            return ret;
        }

        //保存JavLibrary的AvModel
        public async static Task<int> SaveJavLibraryAvModel(AvModel avModel)
        {
            return await new JavLibraryDAL().InsertAvModel(avModel);
        }

        //更新JavLibrary扫描结果的下载状态
        public async static Task<int> UpdateJavLibraryScanDownloadState(int id, bool state)
        {
            return await new JavLibraryDAL().UpdateWebScanUrlModel(id, state);
        }

        //获取JavLibrary的分类，用做全站扫描的入口
        public async static Task<List<CommonModel>> GetJavLibraryCategory()
        {
            List<CommonModel> ret = new List<CommonModel>();
            
            var content = await GetJavLibraryContent(JavLibraryCategoryUrl);

            if (content.exception == null && !string.IsNullOrEmpty(content.content))
            {
                HtmlDocument htmlDocument = new();
                htmlDocument.LoadHtml(content.content);

                var genrePath = "//div[@class='genreitem']";

                var genreNodes = htmlDocument.DocumentNode.SelectNodes(genrePath);

                foreach (var node in genreNodes)
                {
                    var aTagHref = JavLibraryIndexUrl + node.ChildNodes[0].Attributes["href"].Value.Trim();
                    var aTagTitle = node.ChildNodes[0].InnerText.Trim();

                    ret.Add(new CommonModel()
                    {
                        Name = aTagTitle,
                        Type = CommonModelType.Category,
                        Url = aTagHref
                    });
                }
            }

            return ret;
        }

        //获取JavLibrary的列表页信息
        public async static Task<(int pageCount, List<WebScanUrlModel> successList, string fail)> GetJavLibraryListPageInfo(JavLibraryEntryPointType type, string url, int page, bool useExactUrlPassin = false)
        {
            (int, List<WebScanUrlModel>, string) ret = new();
            List<WebScanUrlModel> list = new();
            string fail = "";
            int lastPage = -1;

            var realUrl = "";

            if (useExactUrlPassin)
            {
                realUrl = url;
            }
            else
            {
                realUrl = GetJavLibraryEntryUrl(type, url, page);
            }

            var content = await GetJavLibraryContent(realUrl);

            if (content.exception == null && !string.IsNullOrEmpty(content.content))
            {
                HtmlDocument detailHtmlDocument = new();
                detailHtmlDocument.LoadHtml(content.content);

                var lastPagePath = "//a[@class='page last']";
                var videoPath = "//div[@class='video']";

                var videoNodes = detailHtmlDocument.DocumentNode.SelectNodes(videoPath);
                var lastPageNode = detailHtmlDocument.DocumentNode.SelectSingleNode(lastPagePath);

                if (lastPageNode != null)
                {
                    var pageStr = lastPageNode.Attributes["href"].Value.Trim();

                    if (!string.IsNullOrEmpty(pageStr))
                    {
                        pageStr = pageStr[(pageStr.LastIndexOf("=") + 1)..];

                        int.TryParse(pageStr, out lastPage);
                    }
                }
                else
                {
                    lastPage = 1;
                }

                if (videoNodes != null)
                {
                    foreach (var node in videoNodes)
                    {
                        var urlAndTitle = node.ChildNodes[0];
                        if (urlAndTitle != null && urlAndTitle.ChildNodes.Count >= 3)
                        {
                            var id = urlAndTitle.ChildNodes[0].InnerText.Trim();
                            var name = FileUtility.ReplaceInvalidChar(urlAndTitle.ChildNodes[2].InnerText.Trim());
                            var avUrl = urlAndTitle.Attributes["href"].Value.Trim().Replace("./", JavLibraryIndexUrl);

                            if (!string.IsNullOrEmpty(avUrl) && !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(id))
                            {
                                WebScanUrlModel scan = new WebScanUrlModel
                                {
                                    AvId = id,
                                    IsDownload = false,
                                    Name = name,
                                    URL = avUrl,
                                    ScanUrlSite = WebScanUrlSite.JavLibrary
                                };

                                list.Add(scan);
                            }
                        }
                    }
                }
            }
            else
            {
                fail = realUrl;
            }

            ret.Item1 = lastPage;
            ret.Item2 = list;
            ret.Item3 = fail;

            return ret;
        }

        public async static Task<string> GetListPageName(string url)
        {
            string ret = "";

            var content = await GetJavLibraryContent(url);

            if (content.exception == null && !string.IsNullOrEmpty(content.content))
            {
                HtmlDocument document = new();
                document.LoadHtml(content.content);

                var namePath = "//div[@class='boxtitle']";

                var nameNode = document.DocumentNode.SelectSingleNode(namePath);

                if (nameNode != null)
                {
                    ret = nameNode.InnerText.Split(' ')[0].Trim().Replace("&quot;", "").ToUpper();
                }
            }

            return ret;
        }

        //获取JavLibrary的详情页信息
        public async static Task<(Exception exception, AvModel avModel, List<CommonModel> infos)> GetJavLibraryDetailPageInfo(string url)
        {
            (Exception exception, AvModel avModel, List<CommonModel> infos) ret = new();
            Exception exception = null;
            AvModel avModel = new();
            List<CommonModel> infos = new();

            var content = await GetJavLibraryContent(url);

            if (content.exception == null && !string.IsNullOrEmpty(content.content))
            {
                avModel = GenerateAVModel(content.content, url, infos);
                avModel.Infos = JsonHelper.SerializeWithUtf8(infos);
            }

            ret.exception = exception;
            ret.avModel = avModel;
            ret.infos = infos;

            return ret;
        }

        //获取JavLibrary未下载的Url信息
        public async static Task<List<WebScanUrlModel>> GetJavLibraryWebScanUrlModel(bool onlyNotDownload = true)
        {
            return await new JavLibraryDAL().GetWebScanUrlModel(onlyNotDownload);
        }

        //获取JavLibrary排行榜女优链接
        public async static Task<List<(string name, string url)>> GetRankActressLinks()
        {
            List<(string, string)> ret = new();
            var url = GetJavLibraryEntryUrl(JavLibraryEntryPointType.Rank, "", 1);

            var content = await GetJavLibraryContent(url);

            if (content.exception == null && !string.IsNullOrEmpty(content.content))
            {
                HtmlDocument detailHtmlDocument = new();
                detailHtmlDocument.LoadHtml(content.content);

                var actressListPath = "//div[@class='searchitem']";

                var actressListNodes = detailHtmlDocument.DocumentNode.SelectNodes(actressListPath);

                if (actressListNodes != null)
                {
                    foreach (var node in actressListNodes)
                    {
                        var aHref = node.ChildNodes[1].Attributes["href"].Value.Trim();
                        var name = node.ChildNodes[1].InnerText;

                        (string, string) temp = new();

                        temp.Item1 = name;
                        temp.Item2 = JavLibraryIndexUrl + aHref;

                        ret.Add(temp);
                    }
                }
            }

            return ret;
        }

        //搜索JavLibrary
        public async static Task<List<AvModel>> GetSearchJavLibrary(string content, IProgress<string> progress)
        {
            List<AvModel> ret = new();

            var url = GetJavLibraryEntryUrl(JavLibraryEntryPointType.Search, content, 1);

            var res = await GetJavLibraryContent(url);

            if (res.exception == null && !string.IsNullOrEmpty(res.content))
            {
                if (res.content.Contains("识别码搜寻结果"))
                {
                    var firstPageResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Search, content, 1, false).Result;
                    var totalPage = firstPageResult.pageCount;

                    List<int> pageRange = new();

                    for (int i = 1; i <= totalPage; i++)
                    {
                        pageRange.Add(i);
                    }

                    await Task.Run(() => Parallel.ForEach(pageRange, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
                    {
                        var currentResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Search, content, i, false).Result;

                        if (!string.IsNullOrEmpty(currentResult.fail))
                        {
                            progress.Report($"<=======有失败的URL -> {currentResult.fail}=======>");
                        }

                        if (totalPage > 0 && currentResult.successList != null && currentResult.successList.Count > 0)
                        {
                            foreach (var scan in currentResult.successList)
                            {
                                var tempRes = GetJavLibraryContent(scan.URL).Result;

                                if (tempRes.exception == null && !string.IsNullOrEmpty(tempRes.content))
                                {
                                    List<CommonModel> infos = new();
                                    var avModel = GenerateAVModel(tempRes.content, scan.URL, infos);
                                    avModel.Infos = JsonHelper.SerializeWithUtf8(infos);
                                    ret.Add(avModel);
                                }
                            }
                        }
                    }));
                }
                else
                {
                    HtmlDocument document = new();
                    document.LoadHtml(res.content);

                    var urlPath = "//link[@rel='shortlink']";
                    var urlNode = document.DocumentNode.SelectSingleNode(urlPath);

                    if (urlNode != null)
                    {
                        List<CommonModel> infos = new();
                        var avModel = GenerateAVModel(res.content, urlNode.Attributes["href"].Value.Trim(), infos);
                        avModel.Infos = JsonHelper.SerializeWithUtf8(infos);
                        ret.Add(avModel);
                    }
                }
            }

            return ret;
        }

        ////删除JavLibrary AV的映射关系
        //public async static Task<int> DeleteJavLibraryAvMapping(int avId)
        //{
        //    return await new JavLibraryDAL().DeleteAvMapping(avId);
        //}

        ////插入JavLibrary AV的映射关系
        //public async static Task<int> InsertJavLibraryAvMapping(int avId, CommonJavLibraryModel model)
        //{
        //    return await new JavLibraryDAL().InsertAvMapping(avId, model.Id, model.Type);
        //}

        public static void ScanJavLibraryAllUrlsAndSave(IProgress<string> progress)
        {
            Random ran = new();

            var javLibraryCategories = JavLibraryService.GetJavLibraryCategory().Result;

            Parallel.ForEach(javLibraryCategories, new ParallelOptions { MaxDegreeOfParallelism = 10 }, category =>
            {
                var firstPageResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Category, category.Url, 1).Result;

                var totalPage = firstPageResult.pageCount;

                if (!string.IsNullOrEmpty(firstPageResult.fail))
                {
                    progress.Report($"<=======有失败的URL -> {firstPageResult.fail}=======>");
                }

                if (totalPage > 0 && firstPageResult.successList != null && firstPageResult.successList.Count > 0)
                {
                    foreach (var scan in firstPageResult.successList)
                    {
                        WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                    }

                    for (int i = 2; i <= totalPage; i++)
                    {
                        var tempScanResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Category, category.Url, i).Result;

                        if (!string.IsNullOrEmpty(tempScanResult.fail))
                        {
                            progress.Report($"<=======有失败的URL -> {tempScanResult.fail}=======>");
                        }

                        foreach (var scan in tempScanResult.successList)
                        {
                            WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                        }

                        Task.Delay(ran.Next(50)).Wait();
                    }
                }
            });
        }

        public static void ScanUrlsOccursError(string file, IProgress<string> progress)
        {
            if (File.Exists(file))
            {
                StreamReader sr = new(file);

                while (!sr.EndOfStream)
                {
                    var temp = sr.ReadLine();

                    if (temp.StartsWith(" -- <=======有失败的URL -> "))
                    {
                        temp = temp.Replace(" -- <=======有失败的URL -> ", "").Replace("=======>", "").Trim();

                        var tempScanResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Other, temp, 0, true).Result;

                        if (!string.IsNullOrEmpty(tempScanResult.fail))
                        {
                            progress.Report($"<=======有失败的URL -> {tempScanResult.fail}=======>");
                        }

                        foreach (var scan in tempScanResult.successList)
                        {
                            WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                        }
                    }
                }

                sr.Close();
            }
        }

        public async static Task<int> DownloadJavLibraryDetailAndSavePictureFromWebScanUrl(List<WebScanUrlModel> waitForDownload, IProgress<string> progress)
        {
            int ret = 0;

            var random = new Random();

            var imageFolder = SettingService.GetSetting().Result.JavLibraryImageFolder;

            if (!Directory.Exists(imageFolder))
            {
                Directory.CreateDirectory(imageFolder);
            }

            await Task.Run(() => Parallel.ForEach(waitForDownload, new ParallelOptions { MaxDegreeOfParallelism = 10 }, toBeDownload =>
            {
                var avModelScan = JavLibraryService.GetJavLibraryDetailPageInfo(toBeDownload.URL).Result;

                if (avModelScan.exception == null && avModelScan.avModel != null)
                {
                    var result = 0;
                    try
                    {
                        result = JavLibraryService.SaveJavLibraryAvModel(avModelScan.avModel).Result;
                    }
                    catch (Exception)
                    {
                        progress.Report($"<=====获取 {toBeDownload.URL} 失败=====>");
                    }

                    JavLibraryService.SaveCommonJavLibraryModel(avModelScan.infos).Wait();

                    if (result > 0)
                    {
                        ret++;
                        JavLibraryService.UpdateJavLibraryScanDownloadState(toBeDownload.Id, true).Wait();
                    }

                    var picFile = imageFolder + "\\" + avModelScan.avModel.FileNameWithoutExtension + ".jpg";

                    if (!string.IsNullOrWhiteSpace(avModelScan.avModel.PicUrl) && !File.Exists(picFile))
                    {
                        try
                        {
                            new WebClient().DownloadFile(avModelScan.avModel.PicUrl, picFile);
                        }
                        catch (Exception)
                        {
                            progress.Report($"<=====下载图片 {avModelScan.avModel.PicUrl} 失败=====>");
                        }
                    }
                }
                else
                {
                    progress.Report($"<=====获取 {toBeDownload.URL} 失败=====>");
                }

                Task.Delay(random.Next(50)).Wait();
            }));

            return ret;
        }

        public async static Task<List<WebScanUrlModel>> GetJavLibraryWebScanUrlMode(JavLibraryEntryPointType entry, int pages, string url, bool useExactPassin, string order, IProgress<string> progress)
        {
            List<WebScanUrlModel> scans = new();

            Random ran = new();

            var firstPageResult = JavLibraryService.GetJavLibraryListPageInfo(entry, url, 1, useExactPassin).Result;
            var totalPage = firstPageResult.pageCount;

            progress.Report($"一共有 {totalPage} 页");

            List<int> pageRange = new();

            if (order == JavLibrarySearchOrder.Desc)
            {
                for (int i = 0; i < pages && totalPage - i >= 1; i++)
                {
                    pageRange.Add(totalPage - i);
                }
            }
            else
            {
                for (int i = 1; i <= pages && i <= totalPage; i++)
                {
                    pageRange.Add(i);
                }
            }

            await Task.Run(() => Parallel.ForEach(pageRange, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                progress.Report($"正在扫描第 {i} 页");

                var currentResult = JavLibraryService.GetJavLibraryListPageInfo(entry, url, i, useExactPassin).Result;

                if (!string.IsNullOrEmpty(currentResult.fail))
                {
                    progress.Report($"<=======有失败的URL -> {currentResult.fail}=======>");
                }

                if (totalPage > 0 && currentResult.successList != null && currentResult.successList.Count > 0)
                {
                    foreach (var scan in currentResult.successList)
                    {
                        var id = WebScanCommonService.SaveWebScanUrlModel(scan).Result;

                        if (id > 0)
                        {
                            scan.Id = id;

                            scans.Add(scan);
                        }
                    }
                }

                Task.Delay(ran.Next(50));
            }));

            return scans;
        }
        #endregion

        #region 内部使用
        //获取JavLibrary各个入口的Url
        private static string GetJavLibraryEntryUrl(JavLibraryEntryPointType type, string url, int page)
        {
            string ret = "";

            switch(type)
            {
                case JavLibraryEntryPointType.Actress:
                    ret = url + "&page=" + page;
                    break;

                case JavLibraryEntryPointType.BestRate:
                    ret = "http://www.javlibrary.com/cn/vl_bestrated.php?&mode=&page=" + page;
                    break;

                case JavLibraryEntryPointType.Category:
                    ret = url + "&page=" + page;
                    break;

                case JavLibraryEntryPointType.Company:
                    ret = url + "&page=" + page;
                    break;

                case JavLibraryEntryPointType.Director:
                    ret = url + "&page=" + page;
                    break;

                case JavLibraryEntryPointType.MostWanted:
                    ret = "http://www.javlibrary.com/cn/vl_mostwanted.php?&mode=&page=" + page;
                    break;

                case JavLibraryEntryPointType.Publisher:
                    ret = url + "&page=" + page;
                    break;

                case JavLibraryEntryPointType.Rank:
                    ret = "http://www.javlibrary.com/cn/star_mostfav.php";
                    break;

                case JavLibraryEntryPointType.Update:
                    ret = "http://www.javlibrary.com/cn/vl_update.php?&mode=&page=" + page;
                    break;

                case JavLibraryEntryPointType.Search:
                    ret = "http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=" + url + "&page=" + page;
                    break;

                case JavLibraryEntryPointType.Scan:
                    ret = url + "&page=" + page;
                    break;
            }

            return ret;
        }

        //通用获取JavLibrary的网页内容
        private async static Task<(Exception exception, string content)> GetJavLibraryContent(string url)
        {
            (Exception, string) ret = new();
            var cookie = await GetJavLibraryCookie();
            Exception exception = null;
            string content = "";

            if(cookie.Item1 != null && !string.IsNullOrEmpty(cookie.Item2))
            {
                CookieContainer cc = cookie.Item1;
                var ccStr = "";

                if(cc.Count > 0)
                {
                    ccStr = cc.GetCookieHeader(new Uri(JavLibraryIndexUrl));
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
                        if (ee.StatusCode == HttpStatusCode.Forbidden)
                        {
                            await new JavLibraryDAL().DeleteJavLibraryCookie();
                        }
                    }
                }
            }

            ret.Item1 = exception;
            ret.Item2 = content;

            return ret;     
        }

        //打开Chrome浏览器等待油猴脚本调用API存入Cookie，并退出（有bug）
        private async static Task<(CookieContainer cc, string userAgent)> GetJavCookieChromeProcess()
        {
            var chromeLocation = SettingService.GetSetting().Result.CommonSettings.ChromeLocation;

            if (File.Exists(chromeLocation))
            {
                using(HttpClient client = new ())
                {
                    await client.GetAsync($"http://localhost:20002/job/openbroswer?location={chromeLocation}&url={JavLibraryIndexUrl}");
                }

                await Task.Delay(15 * 1000);

                return await GetJavChromeCookieFromDB();
            }
            
            return new (null, "");
        }

        //使用油猴脚本存入数据库中的Cookie反序列化
        private async static Task<(CookieContainer cc, string userAgent)> GetJavChromeCookieFromDB()
        {
            CookieContainer cc = new();
            string userAgent = "";
            var dbCookieItem = await new JavLibraryDAL().GetJavLibraryCookie();

            if (dbCookieItem != null && !string.IsNullOrEmpty(dbCookieItem.CookieJson) && !string.IsNullOrEmpty(dbCookieItem.UserAgent))
            {
                userAgent = dbCookieItem.UserAgent;

                List<CookieItem> sessionCookieItems = JsonSerializer.Deserialize<List<CookieItem>>(dbCookieItem.CookieJson);

                foreach (var item in sessionCookieItems)
                {
                    Cookie temp = new Cookie(item.Name, item.Value, "/", JavLibraryCookieDomain);
                    cc.Add(temp);
                }
            }

            return new (cc, userAgent);
        }

        private static AvModel GenerateAVModel(string html, string avUrl, List<CommonModel> infos)
        {
            AvModel av = new();

            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);

            var titlePath = "//h3[@class='post-title text']";
            var picPath = "//img[@id='video_jacket_img']";

            var releasdPath = "//div[@id='video_date']//td[@class='text']";
            var lengthPath = "//div[@id='video_length']//span[@class='text']";

            var dirPath = "//span[@class='director']//a";
            var comPath = "//span[@class='maker']//a";
            var pubPath = "//span[@class='label']//a";

            var catPath = "//span[@class='genre']//a";
            var staPath = "//span[@class='star']//a";

            //var reviewPath = "//input[@name='reviewscore' and @checked]";

            var titleNode = htmlDocument.DocumentNode.SelectSingleNode(titlePath);
            var title = titleNode.InnerText.Trim();
            var id = title.Substring(0, title.IndexOf(" "));
            title = FileUtility.ReplaceInvalidChar(title.Substring(title.IndexOf(" ") + 1));
            var picUrl = htmlDocument.DocumentNode.SelectSingleNode(picPath);
            //var reviewNde = htmlDocument.DocumentNode.SelectSingleNode(reviewPath);

            av.Url = avUrl;
            av.PicUrl = picUrl.Attributes["src"].Value;
            av.PicUrl = av.PicUrl.StartsWith("http") ? av.PicUrl : "http:" + av.PicUrl;

            av.Name = title;
            av.AvId = id;
            av.FileNameWithoutExtension = id + "-" + title;

            var release = htmlDocument.DocumentNode.SelectSingleNode(releasdPath);
            DateTime rDate = new(2050, 1, 1);

            if (release != null && !string.IsNullOrEmpty(release.InnerText))
            {
                DateTime.TryParse(release.InnerText.Trim(), out rDate);

                if (rDate <= DateTime.MinValue || rDate >= DateTime.MaxValue)
                {
                    rDate = new(2050, 1, 1);
                }
            }

            av.ReleaseDate = rDate;

            var length = htmlDocument.DocumentNode.SelectSingleNode(lengthPath);
            if (length != null && !string.IsNullOrEmpty(length.InnerText))
            {
                av.AvLength = int.Parse(length.InnerText.Trim());
            }

            var dirNode = htmlDocument.DocumentNode.SelectNodes(dirPath);
            if (dirNode != null)
            {
                foreach (var dir in dirNode)
                {
                    var name = dir.InnerHtml.Trim();
                    var url = JavLibraryIndexUrl + dir.Attributes["href"].Value;

                    infos.Add(new CommonModel()
                    {
                        Name = name,
                        Type = CommonModelType.Director,
                        Url = url
                    });
                }
            }

            var comNode = htmlDocument.DocumentNode.SelectNodes(comPath);
            if (comNode != null)
            {
                foreach (var com in comNode)
                {
                    var name = com.InnerHtml.Trim();
                    var url = JavLibraryIndexUrl + com.Attributes["href"].Value;

                    infos.Add(new CommonModel()
                    {
                        Name = name,
                        Type = CommonModelType.Company,
                        Url = url
                    });
                }
            }

            var pubNode = htmlDocument.DocumentNode.SelectNodes(pubPath);
            if (pubNode != null)
            {
                foreach (var pub in pubNode)
                {
                    var name = pub.InnerHtml.Trim();
                    var url = JavLibraryIndexUrl + pub.Attributes["href"].Value;

                    infos.Add(new CommonModel()
                    {
                        Name = name,
                        Type = CommonModelType.Company,
                        Url = url
                    });
                }
            }

            var catNodes = htmlDocument.DocumentNode.SelectNodes(catPath);
            if (catNodes != null)
            {
                foreach (var cat in catNodes)
                {
                    var name = cat.InnerHtml.Trim();
                    var url = JavLibraryIndexUrl + cat.Attributes["href"].Value;

                    infos.Add(new CommonModel()
                    {
                        Name = name,
                        Type = CommonModelType.Category,
                        Url = url
                    });
                }
            }

            var starNodes = htmlDocument.DocumentNode.SelectNodes(staPath);
            if (starNodes != null)
            {
                foreach (var star in starNodes)
                {
                    var name = star.InnerHtml.Trim();
                    var url = JavLibraryIndexUrl + star.Attributes["href"].Value;

                    infos.Add(new CommonModel()
                    {
                        Name = name,
                        Type = CommonModelType.Actress,
                        Url = url
                    });
                }
            }

            return av;
        }
        #endregion
    }
}