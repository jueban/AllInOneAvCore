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
            Random ran = new();
            Dictionary<string, List<WebScanUrlModel>> scanResult = new();

            var javLibraryCategories = JavLibraryService.GetJavLibraryCategory().Result;

            Parallel.ForEach(javLibraryCategories, new ParallelOptions { MaxDegreeOfParallelism = 10 }, category =>
            {
                Console.WriteLine($"正在处理 {category.Name}");

                List<WebScanUrlModel> temp = new();

                if (scanResult.ContainsKey(category.Name))
                {
                    temp = scanResult[category.Name];
                }
                else
                {
                    scanResult.Add(category.Name, temp);
                }

                var firstPageResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Category, category.Url, 1).Result;

                var totalPage = firstPageResult.Item1;

                if (totalPage > 0 && firstPageResult.Item2 != null && firstPageResult.Item2.Count > 0)
                {
                    Console.WriteLine($"正在处理 {category.Name} 的第 1 页");
                    Console.WriteLine($"{category.Name} 共有 {totalPage} 页");
                    temp.AddRange(firstPageResult.Item2);

                    foreach (var scan in firstPageResult.Item2)
                    {
                        WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                    }

                    for (int i = 2; i <= totalPage; i++)
                    {
                        Console.WriteLine($"正在处理 {category.Name} 的第 {i} / {totalPage} 页");
                        var tempScanResult = JavLibraryService.GetJavLibraryListPageInfo(JavLibraryEntryPointType.Category, category.Url, i).Result;

                        if (tempScanResult.Item1 > 0 && tempScanResult.Item2.Count > 0)
                        {
                            temp.AddRange(tempScanResult.Item2);
                        }

                        foreach (var scan in tempScanResult.Item2)
                        {
                            WebScanCommonService.SaveWebScanUrlModel(scan).Wait();
                        }

                        Task.Delay(ran.Next(3) * 100).Wait();
                    }
                }
            });

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
    }
}