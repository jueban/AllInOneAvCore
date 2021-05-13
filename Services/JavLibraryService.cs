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