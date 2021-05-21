﻿using log4net;
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

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;
            JavLibraryService.ScanJavLibraryAllUrlsAndSave(progress);

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"处理扫描全部Urls完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanAllNotDownloadJavLibraryUrls()
        {
            var waitForDownload = JavLibraryService.GetJavLibraryWebScanUrlModel(true).Result;

            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始处理扫描全部未扫描的AV，共计{waitForDownload.Count}");

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;
            var ret = JavLibraryService.DownloadJavLibraryDetailAndSavePictureFromWebScanUrl(waitForDownload, progress).Result;

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"扫描全部未扫描的AV完成，共下载{ret}, 耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanUrlsOccursError(string file)
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, "开始处理错误丢失URL");

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;
            JavLibraryService.ScanUrlsOccursError(file, progress);

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"处理错误丢失URL完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        public static void ScanJavLibraryUpdateUrls(JavLibraryEntryPointType entry, int pages, string url, bool useExactPassin)
        {
            DateTime startTime = DateTime.Now;
            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始处理最新更新Urls {pages} 页");

            Progress<string> progress = new();
            progress.ProgressChanged += LogInfo;

            var scans = JavLibraryService.GetJavLibraryWebScanUrlMode(entry, pages, url, useExactPassin, progress).Result;

            var ret = JavLibraryService.DownloadJavLibraryDetailAndSavePictureFromWebScanUrl(scans, progress).Result;

            NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始处理最新更新Urls完成 {pages} 页，共下载{ret}, 耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
        }

        private static void LogInfo(object sender, string e)
        {
            LogHelper.Info(e);
        }

    }
}
