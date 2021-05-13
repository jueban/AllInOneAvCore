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

        public async static Task<(CookieContainer, string)> GetJavBusCookie()
        {
            return new(new CookieContainer(), DefaultUserAgent);
        }

        public async static Task<int> SaveCommonJavLibraryModel(List<CommonModel> list)
        {
            int ret = 0;

            foreach (var l in list)
            {
                ret += await new JavBusDAL().InsertCommonJavBusModel(l);
            }

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

        #region 内部使用
        //获取JavLibrary各个入口的Url
        private static string GetJavLibraryEntryUrl(JavBusEntryPointType type, string content, int page)
        {
            string ret = "";

            switch (type)
            {
                case JavBusEntryPointType.HomePage:
                    ret = JavBusIndexUrl + "page=" + page;
                    break;

                case JavBusEntryPointType.Search:
                    ret = JavBusIndexUrl + content + "/" + page;
                    break;

                case JavBusEntryPointType.Category:
                    ret = JavBusIndexUrl + "genre";
                    break;

                case JavBusEntryPointType.Actress:
                    ret = JavBusIndexUrl + "actresses/" + page;
                    break;

                case JavBusEntryPointType.Detail:
                    ret = JavBusIndexUrl + content;
                    break;
            }

            return ret;
        }

        //通用获取JavLibrary的网页内容
        private async static Task<(Exception exception, string content, bool lastPage)> GetJavBusContent(string url)
        {
            (Exception, string, bool) ret = new();
            var cookie = await GetJavBusCookie();
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
        #endregion
    }
}
