using DAL;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class OneOneFiveService
    {
        private static readonly string OneOneFiveDomain = ".115.com";
        private static readonly string OneOneFiveCookieHost = "https://webapi.115.com";

        public async static Task<(CookieContainer, string)> Get115Cookie()
        {
            CookieContainer cc = new CookieContainer();
            string userAgent = "";

            var sessionCookie = await new OneOneFiveDAL().GetOneOneFiveCookie();

            if (sessionCookie != null && !string.IsNullOrEmpty(sessionCookie.CookieJson) && !string.IsNullOrEmpty(sessionCookie.UserAgent))
            {
                userAgent = sessionCookie.UserAgent;

                List<CookieItem> sessionCookieItems = JsonHelper.Deserialize<List<CookieItem>>(sessionCookie.CookieJson);

                foreach (var item in sessionCookieItems)
                {
                    Cookie temp = new Cookie(item.Name, item.Value, "/", "115.com");
                    cc.Add(temp);
                }
            }

            return new(cc, userAgent);
        }

        public async static Task<string> GetOneOneFiveContent(string url)
        {
            string ret = "";

            var cookie = await Get115Cookie();
            CookieContainer cc = cookie.Item1;
            var ccStr = "";

            if (cc.Count > 0)
            {
                ccStr = cc.GetCookieHeader(new Uri(OneOneFiveCookieHost));
            }

            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("Cookie", ccStr);
                client.DefaultRequestHeaders.Add("User-Agent", cookie.Item2);
                ret = await client.GetStringAsync(url);  
            }

            return ret;
        }

        public async static Task<List<OneOneFiveFileItemModel>> Get115SearchFileResult(string content, string folder, bool searchAccurate, int offset = 0, int limit = 1050,  OneOneFiveSearchType searchType = OneOneFiveSearchType.All)
        {
            OneOneFiveFileListModel temp = new OneOneFiveFileListModel();

            string resContent = "";

            var url = string.Format(string.Format($"https://webapi.115.com/files/search?search_value={content}&format=json&offset={offset}&limit={limit}&cid={folder}&type={(int)searchType}"));

            resContent = await GetOneOneFiveContent(url);          

            if (!string.IsNullOrEmpty(resContent))
            {
                temp = JsonConvert.DeserializeObject<OneOneFiveFileListModel>(resContent);
            }


            return (temp == null || temp.data == null) ? null : searchAccurate ? temp.data.Where(x => !string.IsNullOrEmpty(x.fid) && x.n.Contains(content, StringComparison.OrdinalIgnoreCase)).ToList() : temp.data;
        }

        public async static Task<int> SaveOneOneFiveCookie(string cookie, string userAgent)
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

                var httpOnlyCookie = CookieService.Read115Cookie(OneOneFiveDomain);

                if (httpOnlyCookie != null)
                {
                    items.AddRange(httpOnlyCookie);
                }

                if (items != null && items.Count > 0)
                {
                    var business = new OneOneFiveDAL();

                    await business.DeleteOneOneFiveCookie();

                    OneOneFiveCookie entity = new()
                    {
                        CookieJson = JsonHelper.SerializeWithUtf8(items),
                        UserAgent = userAgent
                    };

                    res = await business.InsertOneOneFiveCookie(entity);
                }
            }

            return res;
        }
    }
}
