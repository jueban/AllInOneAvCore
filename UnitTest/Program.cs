using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            JavLibraryService.GetJavLibraryCookie().Wait();

            //DoScanAllJavLibraryFromCategory();
            //DoScanJavLibraryDetail();

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
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

            foreach (var toBeDownload in waitForDownload)
            {
                var avModelScan = JavLibraryService.GetJavLibraryDetailPageInfo(toBeDownload.URL).Result;

                if (avModelScan.exception == null && avModelScan.avModel != null)
                {
                    Console.WriteLine($"正在处理 {toBeDownload.AvId} 一共 {currentIndex} / {totalIndex} ==> {Math.Round((((decimal)currentIndex / totalIndex) * 100), 1) + " %"}");
                    var result = 0;
                    result = JavLibraryService.SaveJavLibraryAvModel(avModelScan.avModel).Result;
                    JavLibraryService.SaveCommonJavLibraryModel(avModelScan.infos).Wait();

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
            }
        }
    }
}