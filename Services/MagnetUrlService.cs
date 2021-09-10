using DAL;
using HtmlAgilityPack;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;
using Dasync.Collections;

namespace Services
{
    public class MagnetUrlService
    {
        public async static Task SaveFaviUrl((WebScanUrlSite site, int type, string url, string name) url)
        {
            await new ScanDAL().InsertFavi(url.site, url.type, url.url, url.name);
        }

        public async static Task<(WebScanUrlSite site, int type, string url, string name)> GetFaviUrl(string url)
        {
            if (url.Contains("javbus.com", StringComparison.OrdinalIgnoreCase))
            {
                var info = PorcessJavBusUrl(url);
                var name = await JavbusService.GetListPageName(url);
                return (WebScanUrlSite.JavBus, info.type, info.url, name);
            }

            if (url.Contains("javlibrary.com", StringComparison.OrdinalIgnoreCase))
            {
                var info = PorcessJavLibraryUrl(url);
                var name = await JavLibraryService.GetListPageName(url);
                return (WebScanUrlSite.JavLibrary, info.type, info.url, name);
            }

            return (WebScanUrlSite.None, -1, "", "");
        }

        public async static Task<List<SeedMagnetSearchModel>> SearchJavLibrary(string url, int page, string name, string order, IProgress<string> progress, string updateUiKey = "", IProgress<(string, int, int)> intProgess = null)
        {
            var startTime = DateTime.Now;
            Random ran = new();

            ScanResult sr = new();
            sr.StartTime = startTime;
            sr.WebSite = WebScanUrlSite.JavLibrary;
            sr.Url = url.Split(',')[0];
            sr.Name = name;
            sr.MagUrl = "";
            sr.Id = await new ScanDAL().SaveSeedMagnetSearchModel(sr);

            progress.Report($"创建ScanResult ID --> {sr.Id}");

            List<SeedMagnetSearchModel> ret = new List<SeedMagnetSearchModel>();

            int pageIndex = 1;

            foreach (var subUrl in url.Split(','))
            {
                progress.Report($"开始扫描{subUrl}");

                var details = await JavLibraryService.GetJavLibraryWebScanUrlMode(JavLibraryEntryPointType.Scan, page, subUrl, false, order, progress);

                progress.Report($"扫描完毕，开始下载磁链");
                int index = 1;

                try
                {
                    await details.ParallelForEachAsync(async id =>
                    {
                        progress.Report($"正在处理 {index++} / {details.Count} ");

                        await JavLibraryService.DownloadJavLibraryDetailAndSavePictureFromWebScanUrl(new List<WebScanUrlModel>() { id }, progress);

                        var res = await SearchSukebeiMag(id.AvId, id.URL);
                        ret.AddRange(res);
                        progress.Report($"\t{id.AvId} 下载了 {res.Count} 个磁链");

                        await Task.Delay(ran.Next(50));
                    });
                }
                catch (Exception)
                {

                }

                if (intProgess != null)
                {
                    intProgess.Report(new(updateUiKey, url.Split(',').Length, pageIndex++));
                }
            }

            sr.MagUrl = JsonHelper.SerializeWithUtf8(ret);

            progress.Report($"更新ScanResult ID --> {sr.Id}");
            await new ScanDAL().UpdateSeedMagnetSearchModel(sr);

            return ret;
        }

        public async static Task<List<SeedMagnetSearchModel>> SearchJavBus(string url, int page, string name, IProgress<string> progress)
        {
            var startTime = DateTime.Now;
            Random ran = new();

            ScanResult sr = new();
            sr.StartTime = startTime;
            sr.WebSite = WebScanUrlSite.JavBus;
            sr.Url = url.Split(',')[0];
            sr.Name = name;
            sr.MagUrl = "";
            sr.Id = await new ScanDAL().SaveSeedMagnetSearchModel(sr);

            progress.Report($"创建ScanResult ID --> {sr.Id}");

            List<SeedMagnetSearchModel> ret = new();

            foreach (var subUrl in url.Split(','))
            {
                progress.Report($"开始扫描{subUrl}");

                var details = await JavbusService.GetJavBusList(JavBusEntryPointType.Passin, subUrl, page, progress, true);
                int index = 1;

                progress.Report($"扫描完毕，开始下载磁链");

                try
                {
                    await details.success.ParallelForEachAsync(async id =>
                    {
                        progress.Report($"正在处理 {index++} / {details.success.Count} ");

                        await JavbusService.GetJavBusDetail(id.URL);
                        List<SeedMagnetSearchModel> res = new List<SeedMagnetSearchModel>();

                        res.AddRange(await SearchSukebeiMag(id.AvId, id.URL));
                        res.AddRange(await SearchJavBusMag(id.AvId, id.URL));

                        ret.AddRange(res);
                        progress.Report($"\t{id.AvId} 下载了 {res.Count} 个磁链");

                        await Task.Delay(ran.Next(50));
                    });
                }
                catch (Exception)
                {

                }
            }

            sr.MagUrl = JsonHelper.SerializeWithUtf8(ret);

            progress.Report($"更新ScanResult ID --> {sr.Id}");
            await new ScanDAL().UpdateSeedMagnetSearchModel(sr);

            return ret;
        }

        public async static Task<List<SeedMagnetSearchModel>> SearchSukebeiMag(string id, string fromUrl)
        {
            List<SeedMagnetSearchModel> ret = new List<SeedMagnetSearchModel>();
            var setting = await SettingService.GetSetting();
            var site = setting.MagSearchSite;

            try
            {
                var searchContent = "";
                var resContent = "";

                if (site == SearchSeedSiteEnum.SukebeiPro)
                {
                    searchContent = "https://sukebei.nyaa.pro/search/c_0_0_k_" + id;
                }
                else if (site == SearchSeedSiteEnum.SukebeiSi)
                {
                    searchContent = "https://sukebei.nyaa.si?f=0&c=0_0&q=" + id;
                }
                else if (site == SearchSeedSiteEnum.SukebeiNet)
                { 
                    searchContent = "https://sukebei.nyaa.net/search?c=_&q=" + id;
                }

                using (HttpClient client = new())
                {
                    try
                    {
                        resContent = await client.GetStringAsync(searchContent);
                    }
                    catch (HttpRequestException)
                    {

                    }
                }

                if (!string.IsNullOrEmpty(resContent))
                {
                    if (site == SearchSeedSiteEnum.SukebeiPro || site == SearchSeedSiteEnum.SukebeiSi)
                    {
                        HtmlDocument htmlDocument = new();
                        htmlDocument.LoadHtml(resContent);

                        string xpath = "//tr";

                        HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

                        foreach (var node in nodes.Skip(1))
                        {
                            var text = FileUtility.ReplaceInvalidChar(node.ChildNodes[3].InnerText.Trim());
                            var a = node.ChildNodes[5].OuterHtml;
                            var size = node.ChildNodes[7].InnerText.Trim();
                            var date = node.ChildNodes[9].OuterHtml.Trim().Replace("<td class=\"text-center\" data-timestamp=\"", "").Replace("\"></td>", "");

                            var url = a.Substring(a.IndexOf("<a href=\"magnet:?xt") + 9);
                            url = url.Substring(0, url.IndexOf("\""));

                            int.TryParse(date, out int seconds);

                            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                            DateTime dt = startTime.AddSeconds(seconds);

                            SeedMagnetSearchModel temp = new SeedMagnetSearchModel
                            {
                                SearchUrl = fromUrl,
                                Title = text,
                                MagSize = FileUtility.GetFileSizeFromString(size),
                                Date = dt,
                                MagUrl = url,
                                Source = site,
                                SerachContent = id
                            };

                            ret.Add(temp);
                        }
                    }
                    else if (site == SearchSeedSiteEnum.SukebeiNet)
                    {
                        HtmlDocument htmlDocument = new();
                        htmlDocument.LoadHtml(resContent);

                        string xpath = "//tbody[@id='torrentListResults']//tr";

                        HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

                        foreach (var node in nodes.Skip(1))
                        {
                            var text = FileUtility.ReplaceInvalidChar(node.ChildNodes[3].InnerText.Trim());
                            var a = node.ChildNodes[5].OuterHtml;
                            var size = node.ChildNodes[7].InnerText.Trim();
                            //var date = node.ChildNodes[9].OuterHtml.Trim().Replace("<td class=\"tr-date home-td date-short hide-xs\" title=\"", "").Replace("\"></td>", "");

                            var url = a.Substring(a.IndexOf("<a href=\"magnet:?xt") + 9);
                            url = url.Substring(0, url.IndexOf("\""));

                            //int.TryParse(date, out int seconds);

                            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                            //DateTime dt = startTime.AddSeconds(seconds);

                            SeedMagnetSearchModel temp = new SeedMagnetSearchModel
                            {
                                SearchUrl = fromUrl,
                                Title = text,
                                MagSize = FileUtility.GetFileSizeFromString(size),
                                //Date = dt,
                                MagUrl = url,
                                Source = site,
                                SerachContent = id
                            };

                            ret.Add(temp);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return ret.OrderByDescending(x => x.MagSize).ToList();
        }

        public async static Task<List<SeedMagnetSearchModel>> SearchJavBusMag(string avId, string fromUrl, CookieContainer cc = null)
        {
            List<SeedMagnetSearchModel> ret = new List<SeedMagnetSearchModel>();

            var refere = "https://www.javbus.com/" + avId;
            var html = "";

            using (HttpClient client = new())
            {
                try
                {
                    html = await client.GetStringAsync(refere);
                }
                catch (HttpRequestException)
                {

                }
            }

            if (!string.IsNullOrEmpty(html))
            {
                var gidPattern = "var gid = (.*?);";
                var ucPattern = "var uc = (.*?);";
                var picPattern = "var img = '(.*?)';";

                var gidMatch = Regex.Match(html, gidPattern);
                var ucMatch = Regex.Match(html, ucPattern);
                var picMatch = Regex.Match(html, picPattern);

                var gid = gidMatch.Groups[1].Value;
                var uc = ucMatch.Groups[1].Value;
                var pic = picMatch.Groups[1].Value;

                var url = $"https://www.javbus.com/ajax/uncledatoolsbyajax.php?gid={gid}&lang=zh&img={pic}&uc={uc}&floor=552";

                var magHtml = "";

                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Add("referer", refere);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36");
                    try
                    {
                        magHtml = await client.GetStringAsync(url);
                    }
                    catch (HttpRequestException)
                    {
                    }
                }

                if (!string.IsNullOrEmpty(magHtml))
                {
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(magHtml);

                    var magPattern = "//tr[@style=' border-top:#DDDDDD solid 1px']";

                    HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes(magPattern);

                    if (nodes != null)
                    {
                        foreach (var node in nodes)
                        {

                            var namePart = "";
                            var sizePart = "";
                            var datePart = "";
                            var magUrl = "";
                            var size = 0d;

                            try
                            {
                                if (node != null)
                                {
                                    if (node.ChildNodes.Count >= 2)
                                    {
                                        namePart = node.ChildNodes[1].InnerText.Trim();
                                        magUrl = node.ChildNodes[1].ChildNodes[1].Attributes["href"].Value;
                                    }

                                    if (node.ChildNodes.Count >= 4)
                                    {
                                        sizePart = node.ChildNodes[3].InnerText.Trim();
                                        size = FileUtility.GetByteFromStr(sizePart);
                                    }

                                    if (node.ChildNodes.Count >= 5)
                                    {
                                        datePart = node.ChildNodes[5].InnerText.Trim();
                                    }

                                    ret.Add(new SeedMagnetSearchModel()
                                    {
                                        CompleteCount = 0,
                                        Date = DateTime.Parse(datePart),
                                        MagSize = size,
                                        MagUrl = magUrl,
                                        Source = SearchSeedSiteEnum.JavBus,
                                        Title = namePart,
                                        SearchUrl = refere,
                                        SerachContent = fromUrl
                                    });
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }

            return ret;
        }

        public async static Task<ScanPageModel> GetScanPageMode(WebScanUrlSite site)
        {
            ScanPageModel ret = new()
            {
                Drops = new List<ScanPageDrop>(),
                Name = "WebScan",
                Page = 9999,
                Url = "",
                Order = JavLibrarySearchOrder.Asc
            };

            if (site == WebScanUrlSite.JavLibrary)
            {
                await DoJavLibraryPageMode(ret);
            }

            if (site == WebScanUrlSite.JavBus)
            {
                await DoJavBusPageMode(ret);
            }

            return ret;
        }

        public async static Task<List<ShowMagnetSearchResult>> GetScanResultDetail(int id)
        {
            List<ShowMagnetSearchResult> ret = new List<ShowMagnetSearchResult>();
            var scanResult = await new ScanDAL().GetSeedMagnetSearchModelById(id);
            var setting = await SettingService.GetSetting();

            if (!string.IsNullOrEmpty(scanResult.MagUrl) && scanResult.MagUrlObj != null)
            {
                var dic = scanResult.MagUrlObj.GroupBy(x => x.SearchUrl).ToDictionary(x => x.Key, x => x.ToList());

                var avModelList = new List<AvModel>();

                if (scanResult.Url.Contains("javlibrary", StringComparison.OrdinalIgnoreCase))
                {
                    avModelList = await new JavLibraryDAL().GetAvModelByWhere("");
                }

                if (scanResult.Url.Contains("javbus", StringComparison.OrdinalIgnoreCase))
                {
                    avModelList = await new JavBusDAL().GetAvModelByWhere("");
                }

                var oneOneFiveAllFiles = JsonConvert.DeserializeObject<List<OneOneFiveFileItemModel>>(RedisService.GetHash("115", "allfiles"));

                foreach (var d in dic)
                {
                    var avModel = avModelList.FirstOrDefault(x => x.Url == d.Key);

                    if (avModel != null)
                    {
                        if (scanResult.Url.Contains("javbus", StringComparison.OrdinalIgnoreCase))
                        {
                            avModel.LocalPic = setting.JavBusImageFolder + avModel.AvId + "-" + avModel.Name + ".jpg";
                        }

                        if (scanResult.Url.Contains("javlibrary", StringComparison.OrdinalIgnoreCase))
                        {
                            avModel.LocalPic = setting.JavLibraryImageFolder + avModel.AvId + "-" + avModel.Name + ".jpg";
                        }

                        ShowMagnetSearchResult temp = new ShowMagnetSearchResult();

                        var magList = d.Value.Where(x => x.Title.Contains(avModel.AvId, StringComparison.OrdinalIgnoreCase)).ToList();

                        if (magList != null && magList.Any())
                        {
                            var matchFiles = await EverythingService.SearchBothLocalAnd115(avModel.AvId, oneOneFiveAllFiles);

                            magList.ForEach(x => x.MagSizeStr = FileUtility.GetAutoSizeString(x.MagSize, 1));

                            temp.FileLocation = FileLocation.None;

                            if (matchFiles != null && matchFiles.Any())
                            {
                                if (matchFiles.Exists(x => x.location == "本地"))
                                {
                                    temp.FileLocation |= FileLocation.Local;
                                    temp.LocalSizeStr = FileUtility.GetAutoSizeString(matchFiles.Where(x => x.location == "本地").Max(x => long.Parse(x.size)), 1);
                                }

                                if (matchFiles.Exists(x => x.location == "115网盘"))
                                {
                                    temp.FileLocation |= FileLocation.OneOneFive;
                                    temp.OneOneFiveSizeStr = FileUtility.GetAutoSizeString(matchFiles.Where(x => x.location == "115网盘").Max(x => long.Parse(x.size)), 1);
                                }

                                temp.BiggestSize = matchFiles.Max(x => long.Parse(x.size));
                                temp.BiggestSizeStr = FileUtility.GetAutoSizeString(temp.BiggestSize, 1);
                                temp.HasChinese = matchFiles.Exists(x => !string.IsNullOrEmpty(x.chinese));
                                temp.HasGreaterSize = magList.Max(x => x.MagSize) > matchFiles.Max(x => long.Parse(x.size));
                                temp.MatchFiles = matchFiles;
                            }
                            else
                            {
                                temp.HasGreaterSize = true;
                                temp.MatchFiles = new();
                            }

                            temp.Magnets = magList.OrderByDescending(x => x.MagSize).Take(5).ToList();
                            temp.AvModel = avModel;

                            ret.Add(temp);
                        }
                    }
                }
            }

            return ret;
        }

        #region 私有方法
        private async static Task DoJavLibraryPageMode(ScanPageModel model)
        {
            model.Drops.Add(new ScanPageDrop()
            {
                Title = "选择页面",
                Items = new List<ScanPageDropItem>() {
                    new ScanPageDropItem() {
                        Text = "更新",
                        Value = "http://www.javlibrary.com/cn/vl_update.php?&mode="
                    },
                    new ScanPageDropItem(){
                        Text = "新加入",
                        Value = "http://www.javlibrary.com/cn/vl_newentries.php?&mode="
                    },
                    new ScanPageDropItem(){
                        Text = "最想要",
                        Value = "http://www.javlibrary.com/cn/vl_mostwanted.php?&mode="
                    },
                    new ScanPageDropItem(){
                        Text = "高评价",
                        Value = "http://www.javlibrary.com/cn/vl_bestrated.php?&mode="
                    }
                }
            });

            var favi = await new ScanDAL().GetFaviByWhere($" AND Site = {(int)WebScanUrlSite.JavLibrary}");

            foreach (var f in favi)
            {
                var temp = model.Drops.FirstOrDefault(x => x.Type == f.type);

                switch ((JavLibraryEntryPointType)f.type)
                {
                    case JavLibraryEntryPointType.Actress:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择演员",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavLibraryEntryPointType.Category:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择类别",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavLibraryEntryPointType.Company:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择公司",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavLibraryEntryPointType.Director:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择导演",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavLibraryEntryPointType.Publisher:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择发行商",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavLibraryEntryPointType.Search:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择前缀",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                }
            }
        }

        private async static Task DoJavBusPageMode(ScanPageModel model)
        {
            model.Drops.Add(new ScanPageDrop()
            {
                Title = "选择页面",
                Items = new List<ScanPageDropItem>() {
                    new ScanPageDropItem() {
                        Text = "更新",
                        Value = "https://www.javbus.com/page"
                    }
                }
            }) ;

            var favi = await new ScanDAL().GetFaviByWhere($" AND Site = {(int)WebScanUrlSite.JavBus}");

            foreach (var f in favi)
            {
                var temp = model.Drops.FirstOrDefault(x => x.Type == f.type);

                switch ((JavBusEntryPointType)f.type)
                {
                    case JavBusEntryPointType.Actress:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择演员",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavBusEntryPointType.Category:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择类别",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavBusEntryPointType.Company:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择公司",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavBusEntryPointType.Director:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择导演",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavBusEntryPointType.Publisher:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择发行商",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                    case JavBusEntryPointType.Search:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择前缀",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;

                    case JavBusEntryPointType.Series:

                        if (temp == null)
                        {
                            model.Drops.Add(new ScanPageDrop()
                            {
                                Type = f.type,
                                Title = "选择系列",
                                Items = new List<ScanPageDropItem>() {
                                    new ScanPageDropItem(){
                                        Value = f.url,
                                        Text = f.name
                                    }
                                }
                            });
                        }
                        else
                        {
                            temp.Items.Add(new ScanPageDropItem()
                            {
                                Value = f.url,
                                Text = f.name
                            });
                        }

                        break;
                }
            }
        }

        private static (int type, string url) PorcessJavLibraryUrl(string url)
        {
            (int type, string url) ret = new();

            if (url.Contains("director"))
            {
                return DoJavLibraryUrl("director", url);
            }

            if (url.Contains("maker"))
            {
                return DoJavLibraryUrl("maker", url);
            }

            if (url.Contains("star"))
            {
                return DoJavLibraryUrl("star", url);
            }

            if (url.Contains("label"))
            {
                return DoJavLibraryUrl("label", url);
            }

            if (url.Contains("genre"))
            {
                return DoJavLibraryUrl("genre", url);
            }

            if (url.Contains("searchbyid"))
            {
                return DoJavLibraryUrl("searchbyid", url);
            }

            return ret;
        }

        private static (int type, string url) PorcessJavBusUrl(string url)
        {
            if (url.Contains("director"))
            {
                return DoJavBusUrl("director", url);
            }

            if (url.Contains("star"))
            {
                return DoJavBusUrl("star", url);
            }

            if (url.Contains("studio"))
            {
                return DoJavBusUrl("studio", url);
            }

            if (url.Contains("label"))
            {
                return DoJavBusUrl("label", url);
            }

            if (url.Contains("genre"))
            {
                return DoJavBusUrl("genre", url);
            }

            if (url.Contains("series"))
            {
                return DoJavBusUrl("series", url);
            }

            if (url.Contains("search"))
            {
                return DoJavBusUrl("search", url);
            }

            return (-1, "");
        }

        private static (int type, string url) DoJavLibraryUrl(string t, string url)
        {
            (int type, string url) ret = new();

            Dictionary<string, JavLibraryEntryPointType> dic = new() { { "director", JavLibraryEntryPointType.Director }, { "star", JavLibraryEntryPointType.Actress }, { "label", JavLibraryEntryPointType.Publisher }, { "maker", JavLibraryEntryPointType.Company }, { "genre", JavLibraryEntryPointType.Category }, { "searchbyid", JavLibraryEntryPointType.Search } };

            Regex reg = new($@"http://www.javlibrary.com/cn/vl_{t}(.+)");
            Match match = reg.Match(url);
            string value = match.Groups[1].Value;

            if (value.Contains("page"))
            {
                ret.url = url.Substring(0, url.LastIndexOf("&"));
            }
            else
            {
                ret.url = url;
            }

            ret.type = (int)dic[t];

            return ret;
        }

        private static (int type, string url) DoJavBusUrl(string t, string url)
        {
            (int type, string url) ret = new();

            Dictionary<string, JavBusEntryPointType> dic = new() { { "director", JavBusEntryPointType.Director }, { "star", JavBusEntryPointType.Actress }, { "label", JavBusEntryPointType.Publisher }, { "studio", JavBusEntryPointType.Company }, { "genre", JavBusEntryPointType.Category }, { "series", JavBusEntryPointType.Series }, { "search", JavBusEntryPointType.Search } };

            Regex reg = new($@"https://www.javbus.com/{t}/(.+)");
            Match match = reg.Match(url);
            string value = match.Groups[1].Value;

            if (value.Contains("/"))
            {
                ret.url = url.Substring(0, url.LastIndexOf("/"));
            }
            else
            {
                ret.url = url;
            }

            ret.type = (int)dic[t];

            return ret;
        }
        #endregion
    }
}