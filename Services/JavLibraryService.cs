using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
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
        private static readonly string ChromeLocation = @"C:\Program Files\Google\Chrome\Application\Chrome.exe";
        private static readonly string JavLibraryCookieDomain = ".javlibrary.com";
        private static readonly string JavLibraryIndexUrl = "http://www.javlibrary.com/cn/";
        private static readonly string JavLibraryCategoryUrl = "http://www.javlibrary.com/cn/genres.php";
        #endregion

        #region Cookie
        //获取JavLibraryCookie
        public async static Task<ValueTuple<CookieContainer, string>> GetJavLibraryCookie()
        {
            string content = "";
            string userAgent = "";
            CookieContainer ret = null;

            using (HttpClient client = new HttpClient())
            {
                content = await client.GetStringAsync("http://localhost:10001/api/config/GetConfigModel?site=JavLibrarySetting&key=CookieMode");
            }
            
            var jsonObj = JsonSerializer.Deserialize<ConfigModel>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            var mode = JavLibraryGetCookieMode.Easy;

            if(jsonObj != null)
            {
                mode = (JavLibraryGetCookieMode)Enum.Parse(typeof(JavLibraryGetCookieMode), jsonObj.Value, true);
            }
            
            if(mode == JavLibraryGetCookieMode.MockBroswer)
            {
                //TODO 如果有需要JavLibraryCookie的接口调用失败（认为Cookie失效）则删除数据库中Cookie记录，走False分支
                if(new JavLibraryDAL().GetJavLibraryCookie().Result != null)
                {
                    var res = await GetJavChromeCookieFromDB();

                    if(res.Item1 != null && !string.IsNullOrEmpty(res.Item2))
                    {
                        ret = res.Item1;
                        userAgent = res.Item2;
                    }
                }
                else
                {
                    var res = await GetJavCookieChromeProcess();

                    if(res.Item1 != null && !string.IsNullOrEmpty(res.Item2))
                    {
                        ret = res.Item1;
                        userAgent = res.Item2;
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
            List<CookieItem> items = new List<CookieItem>();        
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

                    JavLibraryCookieJson entity = new JavLibraryCookieJson()
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
        public async static Task<int> SaveCommonJavLibraryModel(List<CommonJavLibraryModel> list)
        {
            int ret = 0;

            foreach(var l in list)
            {
                ret += await new JavLibraryDAL().InsertCommonJavLibraryModel(l);
            }

            return ret;
        }

        //获取JavLibrary的分类，用做全站扫描的入口
        public async static Task<List<CommonJavLibraryModel>> GetJavLibraryCategory()
        {
            List<CommonJavLibraryModel> ret = new List<CommonJavLibraryModel>();
            
            var content = await GetJavLibraryContent(JavLibraryCategoryUrl);

            if (content.Item1 == null && !string.IsNullOrEmpty(content.Item2))
            {
                HtmlDocument htmlDocument = new();
                htmlDocument.LoadHtml(content.Item2);

                var genrePath = "//div[@class='genreitem']";

                var genreNodes = htmlDocument.DocumentNode.SelectNodes(genrePath);

                foreach (var node in genreNodes)
                {
                    var aTagHref = JavLibraryIndexUrl + node.ChildNodes[0].Attributes["href"].Value.Trim();
                    var aTagTitle = node.ChildNodes[0].InnerText.Trim();

                    ret.Add(new CommonJavLibraryModel()
                    {
                        Name = aTagTitle,
                        Type = CommonJavLibraryModelType.Category,
                        Url = aTagHref
                    });
                }
            }

            return ret;
        }

        //获取JavLibrary的列表页信息
        public async static Task<ValueTuple<int, List<WebScanUrlModel>>> GetJavLibraryListPageInfo(JavLibraryEntryPointType type, string url, int page)
        {
            ValueTuple<int, List<WebScanUrlModel>> ret = new();
            List<WebScanUrlModel> list = new(); 
            int lastPage = -1;
            var content = await GetJavLibraryContent(GetJavLibraryEntryUrl(type, url, page));

            if(content.Item1 == null && !string.IsNullOrEmpty(content.Item2))
            {
                HtmlDocument detailHtmlDocument = new();
                detailHtmlDocument.LoadHtml(content.Item2);

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
                                    Id = id,
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

            ret.Item1 = lastPage;
            ret.Item2 = list;

            return ret;
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
            }

            return ret;
        }

        //通用获取JavLibrary的网页内容
        private async static Task<ValueTuple<Exception, string>> GetJavLibraryContent(string url)
        {
            ValueTuple<Exception, string> ret = new();
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

        //打开Chrome浏览器等待油猴脚本调用API存入Cookie，并退出（有bug）
        private async static Task<ValueTuple<CookieContainer, string>> GetJavCookieChromeProcess()
        {
            if(File.Exists(ChromeLocation))
            {
                var process = System.Diagnostics.Process.Start(ChromeLocation, JavLibraryIndexUrl);

                await Task.Delay(10 * 1000);

                if(process != null)
                {
                    process.Kill();
                }

                return await GetJavChromeCookieFromDB();
            }
            
            return new (null, "");
        }

        //使用油猴脚本存入数据库中的Cookie反序列化
        private async static Task<ValueTuple<CookieContainer, string>> GetJavChromeCookieFromDB()
        {
            CookieContainer cc = new CookieContainer();
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
        #endregion
    }
}
