using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using Models;
using Services;
using Utils;

namespace UnitTest
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            //JavLibraryService.GetJavLibraryCookie().Wait();
            //var res = JavLibraryService.GetRankActressLinks().Result;

            //DoScanAllJavLibraryFromCategory();
            //DoScanJavLibraryDetail();

            //GetJavLibraryWebScanUrlMode(JavLibraryEntryPointType.Update, 3, "", false);

            //var category = JavbusService.GetJavBusCategory().Result;

            //JavbusService.SaveCommonJavLibraryModel(category).Wait();

            //var actress = JavbusService.GetJavBusActress().Result;
            //JavbusService.SaveCommonJavLibraryModel(actress.actress).Wait();

            //Parallel.ForEach(actress.pics, new ParallelOptions { MaxDegreeOfParallelism = 10 }, pic =>
            //{
            //    if (!File.Exists(pic.file))
            //    {
            //        try
            //        {
            //            new System.Net.WebClient().DownloadFile(pic.url, pic.file);
            //        }
            //        catch (Exception)
            //        {
            //            LogHelper.Info($"<=====下载图片 {pic.url} 失败=====>");
            //        }
            //    }
            //});

            //var av = JavbusService.GetJavBusDetail("https://www.javbus.com/vdd-100").Result;

            //var av = new JavLibraryDAL().GetAvModelByWhere($" AND AvId='{"vdd-100"}'").Result;

            //var avs = JavLibraryService.GetSearchJavLibrary("vdd-10").Result;

            //FileUtility.RenameAndTransferUsingSystem(@"N:\Download\movefiles\fin\DMAT-192-眠る義母 息子に夜●いされて (2).mp4", @"N:\Download\movefiles\DMAT-192-眠る義母 息子に夜●いされて (2).mp4", true);

            var ret = EverythingService.EverythingSearch("vdd-100").Result;

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }

        private static void PrintLog(object sender, string e)
        {
            Console.WriteLine(e);
        }

        //测试通过分类信息扫描全部JavLibrary
        static void DoScanAllJavLibraryFromCategory()
        {
            Random ran = new();
            List<string> failUrls = new();

            var javLibraryCategories = JavLibraryService.GetJavLibraryCategory().Result;

            int currentIndex = 1;
            int totalIndex = javLibraryCategories.Count;

            Parallel.ForEach(javLibraryCategories, new ParallelOptions { MaxDegreeOfParallelism = 10 }, category =>
            {
                Console.WriteLine($"正在处理 {category.Name} 的第 1 页");

                var firstPageResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Category, category.Url, 1).Result;

                var totalPage = firstPageResult.pageCount;

                if (!string.IsNullOrEmpty(firstPageResult.fail))
                {
                    failUrls.Add(firstPageResult.fail);
                    Console.WriteLine($"<=======有失败的URL -> {firstPageResult.fail}=======>");
                }

                if (totalPage > 0 && firstPageResult.successList != null && firstPageResult.successList.Count > 0)
                {
                    Console.WriteLine($"{category.Name} 共有 {totalPage} 页");

                    foreach (var scan in firstPageResult.successList)
                    {
                        WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                    }

                    for (int i = 2; i <= totalPage; i++)
                    {
                        Console.WriteLine($"正在处理 {category.Name} 的第 {i} / {totalPage} 页  ==> {Math.Round((((decimal)i/totalPage) * 100), 1) + " %"}  总分类 {currentIndex} / {totalIndex} ==> {Math.Round((((decimal)currentIndex / totalIndex) * 100), 1) + " %"}");
                        var tempScanResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Category, category.Url, i).Result;

                        if (!string.IsNullOrEmpty(tempScanResult.fail))
                        {
                            failUrls.Add(tempScanResult.fail);
                            Console.WriteLine($"<=======有失败的URL -> {tempScanResult.fail}=======>");
                        }

                        foreach (var scan in tempScanResult.successList)
                        {
                            WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                        }

                        Task.Delay(ran.Next(100)).Wait();
                    }
                }

                currentIndex++;
            });
        }

        //测试扫描JavLibrary详情页
        static void DoScanJavLibraryDetail()
        {
            var waitForDownload = JavLibraryService.GetJavLibraryWebScanUrlModel(true).Result;

            int currentIndex = 1;
            int totalIndex = waitForDownload.Count;

            Parallel.ForEach(waitForDownload, new ParallelOptions { MaxDegreeOfParallelism = 10 }, toBeDownload =>
             {
                 var avModelScan = JavLibraryService.GetJavLibraryDetailPageInfo(toBeDownload.URL).Result;

                 if (avModelScan.exception == null && avModelScan.avModel != null)
                 {
                     Console.WriteLine($"正在处理 {toBeDownload.AvId} 一共 {currentIndex} / {totalIndex} ==> {Math.Round((((decimal)currentIndex / totalIndex) * 100), 1) + " %"}");
                     var result = 0;
                     try
                     {
                         result = JavLibraryService.SaveJavLibraryAvModel(avModelScan.avModel).Result;
                         JavLibraryService.SaveCommonJavLibraryModel(avModelScan.infos).Wait();
                     }
                     catch (Exception e)
                     {
                         Console.WriteLine(e.ToString());
                     }

                     if (result > 0)
                     {
                         JavLibraryService.UpdateJavLibraryScanDownloadState(toBeDownload.Id, true).Wait();
                     }

                     currentIndex++;
                 }
                 else
                 {
                     Console.WriteLine($"<=====获取 {toBeDownload.AvId} 失败=====>");
                 }
             });
        }

        static List<WebScanUrlModel> GetJavLibraryWebScanUrlMode(JavLibraryEntryPointType entry, int pages, string url, bool useExactPassin)
        {
            List<WebScanUrlModel> scans = new();

            var firstPageResult = JavLibraryService.GetJavLibraryListPageInfo(entry, url, 1, useExactPassin).Result;
            var totalPage = firstPageResult.pageCount;

            List<int> pageRange = new();

            for (int i = 1; i <= pages && i <= totalPage; i++)
            {
                pageRange.Add(i);
            }

            Task.Run(() => Parallel.ForEach(pageRange, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                var currentResult = JavLibraryService.GetJavLibraryListPageInfo(entry, url, i, useExactPassin).Result;

                if (!string.IsNullOrEmpty(currentResult.fail))
                {
                    LogHelper.Info($"<=======有失败的URL -> {currentResult.fail}=======>");
                }

                if (totalPage > 0 && currentResult.successList != null && currentResult.successList.Count > 0)
                {
                    foreach (var scan in currentResult.successList)
                    {
                        var id = WebScanCommonService.SaveWebScanUrlModel(scan).Result;

                        if (id > 0)
                        {
                            scan.Id = id;

                            scans.Add(scan);
                        }
                    }
                }
            })).Wait();

            return scans;
        }
    }

    public class ScanParam
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
    }
}