using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AvManager.Hangfire
{
    public class Jobs
    {
        public static void OpenBroswerJob()
        {
            NoticeService.SendBarkNotice("开始打开浏览器获取Cookie");

            ScheduleService.RunScheduler("OpenJavLibraryToGetCookie");
        }

        public static void ScanJavLibraryAllUrlsAndSave()
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice("开始处理扫描全部Urls");

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;
            JavLibraryService.ScanJavLibraryAllUrlsAndSave(progress).Wait();

            NoticeService.SendBarkNotice( $"处理扫描全部Urls完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanAllNotDownloadJavLibraryUrls()
        {
            var waitForDownload = JavLibraryService.GetJavLibraryWebScanUrlModel(true).Result;

            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice( $"开始处理扫描全部未扫描的AV，共计{waitForDownload.Count}");

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;
            var ret = JavLibraryService.DownloadJavLibraryDetailAndSavePictureFromWebScanUrl(waitForDownload, progress).Result;

            NoticeService.SendBarkNotice( $"扫描全部未扫描的AV完成，共下载{ret}, 耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanUrlsOccursError(string file)
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice("开始处理错误丢失URL");

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;
            JavLibraryService.ScanUrlsOccursError(file, progress);

            NoticeService.SendBarkNotice( $"处理错误丢失URL完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void GenerateReport()
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice("开始生成报表");

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;
            ReportService.GenerateReport().Wait();

            NoticeService.SendBarkNotice( $"生成报表完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanJavLibraryUpdateUrls(JavLibraryEntryPointType entry, int pages, string url, bool useExactPassin)
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice( $"开始处理最新更新Urls {pages} 页");

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;

            var scans = JavLibraryService.GetJavLibraryWebScanUrlMode(entry, pages, url, useExactPassin, JavLibrarySearchOrder.Asc, progress).Result;

            var ret = JavLibraryService.DownloadJavLibraryDetailAndSavePictureFromWebScanUrl(scans, progress).Result;

            NoticeService.SendBarkNotice( $"开始处理最新更新Urls完成 {pages} 页，共下载{ret}, 耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        private static void LogInfo(object sender, string e)
        {
            LogHelper.Info(e);
        }

        public static void ScanJavUpdate(string site, int page)
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice( $"开始扫描 {site} 磁链");
            Progress<string> progress = new Progress<string>();
            progress.ProgressChanged += LogInfo;

            if (site == "javlibrary")
            {
                MagnetUrlService.SearchJavLibrary("http://www.javlibrary.com/cn/vl_update.php?&mode=", page, "Siri扫描Javlibrary", JavLibrarySearchOrder.Asc, progress).Wait();
            }

            if (site == "javbus")
            {
                MagnetUrlService.SearchJavBus("https://www.javbus.com/page", page, "Siri扫描Javbus", progress).Wait();
            }

            NoticeService.SendBarkNotice( $"结束扫描 {site} 磁链, 用时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public async static void PingService()
        {
            var setting = await SettingService.GetSetting();

            var sites = setting.PingServiceSite.Split(',').ToList();

            foreach (var site in sites)
            {
                using (HttpClient hc = new HttpClient())
                {
                    var result = await hc.GetStringAsync(site + "/ping/ping");
                }
            }
        }

        public static void GeneratePotPlayerListAndPlay(string key)
        {
            List<string> files = JsonConvert.DeserializeObject<List<string>>(RedisService.GetHash("play", key));

            var folder = "c:\\setting\\playlist\\";
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "PlayList.dpl";
            var sb = new StringBuilder();
            int index = 1;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.Create(folder + fileName).Close();

            sb.AppendLine("DAUMPLAYLIST");

            foreach (var f in files)
            {
                sb.AppendLine(string.Format("{0}*file*{1}", index++, f));
                sb.AppendLine("1*played*0");
            }

            using (StreamWriter sw = new StreamWriter(folder + fileName))
            {
                sw.WriteLine(sb.ToString());
            }

            ScheduleService.CreateOrReCreateOneTimeSchedulerAndRun("PlayPotPlayerList", "PlayPotPlayerPlayListThatGenerateByProgram", @"C:\Program Files\DAUM\PotPlayer\PotPlayerMini64.exe", folder + fileName);
        }
    }
}
