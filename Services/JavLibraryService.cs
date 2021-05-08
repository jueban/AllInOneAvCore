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

namespace Services
{
    public class JavLibraryService
    {
        private static readonly string ChromeLocation = @"C:\Program Files\Google\Chrome\Application\Chrome.exe";
        private static readonly string JavLibraryCookieDomain = ".javlibrary.com";
        private static readonly string JavLibraryIndexUrl = "http://www.javlibrary.com/cn/";
        private static readonly string JavLibraryCategoryUrl = "http://www.javlibrary.com/cn/genres.php";
        private static readonly string JavLibrarySearchUrl = "http://www.javlibrary.com/cn/";


        #region Cookie
        //获取JavLibraryCookie
        public async static Task<CookieContainer> GetJavLibraryCookie()
        {
            string content = "";

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

            CookieContainer ret = null;
            
            if(mode == JavLibraryGetCookieMode.MockBroswer)
            {
                //TODO 如果有需要JavLibraryCookie的接口调用失败（认为Cookie失效）则删除数据库中Cookie记录，走False分支
                if(new JavLibraryDAL().GetJavLibraryCookie().Result != null)
                {
                    ret = await GetJavChromeCookieFromDB();
                }
                else
                {
                    ret = await GetJavCookieChromeProcess();
                }
            }

            if (mode == JavLibraryGetCookieMode.Easy)
            {
                ret = new CookieContainer();
            }

            return ret;
        }

        //读取从油猴脚本传过来的Cookie再结合上从本地Chrome读取到的HttpOnly的Cookie
        public async static Task<int> SaveJavLibraryCookie(string cookie)
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
                        CookieJson = JsonSerializer.Serialize(items)
                    };

                    res = await business.InsertJavLibraryCookie(entity);
                }
            }

            return res;
        }
        #endregion

        #region 网页
        public async static Task<List<CommonJavLibraryModel>> DownloadCategory()
        {
            List<CommonJavLibraryModel> ret = new List<CommonJavLibraryModel>();
            var cc = await GetJavLibraryCookie();
            var ccStr = cc.GetCookieHeader(new Uri(JavLibraryIndexUrl));

            string content = "";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Cookie", ccStr);
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36");
                content = await client.GetStringAsync(JavLibraryCategoryUrl);
            }

            if (!string.IsNullOrEmpty(content))
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(content);

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
        #endregion

        #region 内部使用
        //打开Chrome浏览器等待油猴脚本调用API存入Cookie，并退出（有bug）
        private async static Task<CookieContainer> GetJavCookieChromeProcess()
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
            
            return null;
        }

        //使用油猴脚本存入数据库中的Cookie反序列化
        private async static Task<CookieContainer> GetJavChromeCookieFromDB()
        {
            CookieContainer cc = new CookieContainer();
            var dbCookieItem = await new JavLibraryDAL().GetJavLibraryCookie();

            if (dbCookieItem != null && !string.IsNullOrEmpty(dbCookieItem.CookieJson))
            {
                List<CookieItem> sessionCookieItems = JsonSerializer.Deserialize<List<CookieItem>>(dbCookieItem.CookieJson);

                foreach (var item in sessionCookieItems)
                {
                    Cookie temp = new Cookie(item.Name, item.Value, "/", JavLibraryCookieDomain);
                    cc.Add(temp);
                }
            }

            return cc;
        }
        #endregion
    }
}
