using log4net;
using Microsoft.Extensions.Logging;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace Hangfire
{
    public class Jobs
    {
        public static void OpenBroswerJob(string location, string url)
        {
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, "开始打开浏览器获取Cookie");

            ScheduleService.RunScheduler("OpenJavLibraryToGetCookie");
        }

        //没想好怎么搞processId
        public static void CloseBroswerJob(int processId)
        {
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, "开始关闭浏览器获取Cookie");

            System.Diagnostics.Process.GetProcessById(processId);
        }

        public static void ScanJavLibraryAllUrlsAndSave()
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, "开始处理扫描全部Urls");

            Random ran = new();

            var javLibraryCategories = JavLibraryService.GetJavLibraryCategory().Result;

            Parallel.ForEach(javLibraryCategories, new ParallelOptions { MaxDegreeOfParallelism = 10 }, category =>
            {
                var firstPageResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Category, category.Url, 1).Result;

                var totalPage = firstPageResult.pageCount;

                if (!string.IsNullOrEmpty(firstPageResult.fail))
                {
                    LogHelper.Info($"<=======有失败的URL -> {firstPageResult.fail}=======>");
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
                            LogHelper.Info($"<=======有失败的URL -> {tempScanResult.fail}=======>");
                        }

                        foreach (var scan in tempScanResult.successList)
                        {
                            WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                        }

                        Task.Delay(ran.Next(50)).Wait();
                    }
                }
            });

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"处理扫描全部Urls完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanAllNotDownloadJavLibraryUrls()
        {
            var waitForDownload = JavLibraryService.GetJavLibraryWebScanUrlModel(true).Result;

            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始处理扫描全部未扫描的AV，共计{waitForDownload.Count}");

            var ret = Helper.DownloadJavLibraryDetailAndSavePicture(waitForDownload).Result;

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"扫描全部未扫描的AV完成，共下载{ret}, 耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanUrlsOccursError(string file)
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, "开始处理错误丢失URL");

            if (File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);

                while (!sr.EndOfStream)
                {
                    var temp = sr.ReadLine();

                    if (temp.StartsWith(" -- <=======有失败的URL -> "))
                    {
                        temp = temp.Replace(" -- <=======有失败的URL -> ", "").Replace("=======>", "").Trim();

                        var tempScanResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Other, temp, 0, true).Result;

                        if (!string.IsNullOrEmpty(tempScanResult.fail))
                        {
                            LogHelper.Info($"<=======有失败的URL -> {tempScanResult.fail}=======>");
                        }

                        foreach (var scan in tempScanResult.successList)
                        {
                            WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                        }
                    }
                }

                sr.Close();
            }

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"处理错误丢失URL完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanJavLibraryUpdateUrls(JavLibraryEntryPointType entry, int pages, string url, bool useExactPassin)
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始处理最新更新Urls {pages} 页");

            var scans = Helper.GetJavLibraryWebScanUrlMode(entry, pages, url, useExactPassin).Result;

            var ret = Helper.DownloadJavLibraryDetailAndSavePicture(scans).Result;

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始处理最新更新Urls完成 {pages} 页，共下载{ret}, 耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }
    }
}
