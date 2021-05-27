﻿using DAL;
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

        public async static Task<List<OneOneFiveFileItemModel>> Get115SearchFileResult(string content, string folder, bool searchAccurate, int offset = 0, int limit = 1050, OneOneFiveSearchType searchType = OneOneFiveSearchType.All)
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

        public async static Task<OneOneFiveResult> AddOneOneFiveTask(string mag)
        {
            var split = mag.Split(new string[] { "magnet:?" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x));
            var url = "";

            Dictionary<string, string> param = new Dictionary<string, string>();

            if (split.Count() <= 1)
            {
                param.Add("url", mag.Trim());
            }
            else
            {
                int index = 0;
                foreach (var s in split)
                {
                    param.Add(string.Format("url[{0}]", index), "magnet:?" + s);

                    index++;
                }
            }

            param.Add("sign", "");
            param.Add("uid", "340200422");
            param.Add("time", DateTime.Now.ToFileTimeUtc() + "");

            if (split.Count() <= 1)
            {
                url = "https://115.com/web/lixian/?ct=lixian&ac=add_task_url";
            }
            else
            {
                url = "https://115.com/web/lixian/?ct=lixian&ac=add_task_urls";
            }

            var res = await OneOneFiveTaskPost(url, param);

            return new OneOneFiveResult()
            {
                error_msg = res.msg,
                state = res.status
            };
        }

        public async static Task<(bool status, string msg)> OneOneFiveTaskPost(string url, Dictionary<string, string> param)
        {
            (bool status, string msg) ret = new();

            var cookie = await Get115Cookie();
            CookieContainer cc = cookie.Item1;
            var ccStr = "";

            if (cc.Count > 0)
            {
                ccStr = cc.GetCookieHeader(new Uri(OneOneFiveCookieHost));
            }

            HttpResponseMessage res = null;

            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                client.DefaultRequestHeaders.Add("Cookie", ccStr);
                client.DefaultRequestHeaders.Add("User-Agent", cookie.Item2);
                HttpContent hc = new FormUrlEncodedContent(param);
                hc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                res = await client.PostAsync(url, hc);
            }

            if (res != null)
            {
                var data = JsonConvert.DeserializeObject<OneOneFiveResult>(res.Content.ReadAsStringAsync().Result);

                if (data.state)
                {
                    ret.status = true;
                }
                else
                {
                    ret.msg = data.error_msg;
                }
            }

            return ret;
        }

        public async static Task<int> Get115PagesInFolder(OneOneFiveSearchType type, string folder, int pageSize = 1)
        {
            var url = $"https://webapi.115.com/files?aid=1&cid={folder}&o=user_ptime&asc=0&offset=0&show_dir=1&limit={pageSize}&code=&scid=&snap=0&natsort=1&record_open_time=1&source=&format=json&type={((int)type).ToString()}";

            var res = await GetOneOneFiveContent(url);
            if (!string.IsNullOrEmpty(res))
            {
                var data = JsonConvert.DeserializeObject<OneOneFiveFileListModel>(res);

                if (data != null && data.count > 0)
                {
                    if (data.data == null)
                    {
                        return data.count % pageSize == 0 ? data.count / pageSize : data.count / pageSize + 1;
                    }

                    return data.count % data.page_size == 0 ? data.count / data.page_size : data.count / data.page_size + 1;
                }
            }

            return 0;
        }

        public async static Task<OneOneFiveFileListModel> GetMixedOneOneFileInFolder(string folder, int page = 0, int pageSize = 1150)
        {
            OneOneFiveFileListModel ret = new OneOneFiveFileListModel();

            var url = $"https://aps.115.com/natsort/files.php?aid=1&cid={folder}&o=file_name&asc=1&offset={page}&show_dir=1&limit={pageSize}&code=&scid=&snap=0&natsort=1&record_open_time=1&source=&format=json&fc_mix=0";

            var res = await GetOneOneFiveContent(url);

            if (!string.IsNullOrEmpty(res))
            {
                ret = JsonConvert.DeserializeObject<OneOneFiveFileListModel>(res);
            }

            return ret;
        }

        public async static Task<OneOneFiveFileListModel> GetOneOneFileInFolder(string folder, OneOneFiveSearchType type, int page = 0, int pageSize = 1150)
        {
            OneOneFiveFileListModel ret = new();

            var url = $"https://webapi.115.com/files?aid=1&cid={folder}&o=user_ptime&asc=0&offset={page}&show_dir=1&limit={pageSize}&code=&scid=&snap=0&natsort=1&record_open_time=1&source=&format=json&type={((int)type).ToString()}&star=&is_q=&is_share=";

            var res = await GetOneOneFiveContent(url);

            if (!string.IsNullOrEmpty(res))
            {
                ret = JsonConvert.DeserializeObject<OneOneFiveFileListModel>(res);

                if (ret.data == null)
                {
                    ret = await GetMixedOneOneFileInFolder(folder, page, pageSize);
                }
            }

            return ret;
        }

        public async static Task<List<OneOneFiveFileItemModel>> Get115AllFilesModel(string folder, OneOneFiveSearchType type = OneOneFiveSearchType.All)
        {
            List<OneOneFiveFileItemModel> list = new List<OneOneFiveFileItemModel>();

            var pages = await Get115PagesInFolder(type, folder, 1050);

            for (int i = 0; i < pages; i++)
            {
                var files = await GetOneOneFileInFolder(folder, type, i * 1150, 1150);

                if (files != null && files.data != null)
                {
                    list.AddRange(files.data);
                }
            }

            return list;
        }

        public async static Task RefreshOneOneFiveFinFilesCache()
        {
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始更新115文件缓存");
            var startTime = DateTime.Now;

            var ret = await Get115AllFilesModel(OneOneFiveFolder.Fin, OneOneFiveSearchType.Video);

            RedisService.DeleteHash("115", "allfiles");

            RedisService.SetHash("115", "allfiles", JsonConvert.SerializeObject(ret));

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"更新115文件缓存结束，更新了 {ret.Count} 条，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
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
    }
}
