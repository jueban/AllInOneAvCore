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
            Progress<string> progress = new Progress<string>();
            progress.ProgressChanged += PrintLog;

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
            //MagnetUrlService.SearchJavBus("https://www.javbus.com/page", 1, "Siri扫描Javbus", progress).Wait();

            var input = @"http://www.javlibrary.com/cn/vl_director.php?d=a4eq
                            http://www.javlibrary.com/cn/vl_star.php?s=aerti
                            http://www.javlibrary.com/cn/vl_star.php?s=ayote
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=rbd
                            http://www.javlibrary.com/cn/vl_star.php?s=aznsy
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=rki
                            http://www.javlibrary.com/cn/vl_star.php?s=krea
                            http://www.javlibrary.com/cn/vl_star.php?s=ae2tm
                            http://www.javlibrary.com/cn/vl_star.php?s=azfuu
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=ddt
                            http://www.javlibrary.com/cn/vl_star.php?s=brjq
                            http://www.javlibrary.com/cn/vl_star.php?s=anha
                            http://www.javlibrary.com/cn/vl_genre.php?g=ma
                            http://www.javlibrary.com/cn/vl_star.php?s=afib6
                            http://www.javlibrary.com/cn/vl_genre.php?&mode=&g=aqoq
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=soan
                            http://www.javlibrary.com/cn/vl_star.php?s=azfr2
                            http://www.javlibrary.com/cn/vl_star.php?s=afctm
                            http://www.javlibrary.com/cn/vl_star.php?s=ayias
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=JBD
                            http://www.javlibrary.com/cn/vl_star.php?s=aeaf2
                            http://www.javlibrary.com/cn/vl_star.php?s=aymqk
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=mvsd
                            http://www.javlibrary.com/cn/vl_star.php?s=aesbo
                            http://www.javlibrary.com/cn/vl_star.php?s=azosy
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=bueq
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=xrw
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=NHDTB
                            http://www.javlibrary.com/cn/vl_star.php?s=mfiq
                            http://www.javlibrary.com/cn/vl_star.php?s=afduu
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=mism
                            http://www.javlibrary.com/cn/vl_genre.php?g=ie
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=ae5s6
                            http://www.javlibrary.com/cn/vl_star.php?s=aewu2
                            http://www.javlibrary.com/cn/vl_star.php?s=azec6
                            http://www.javlibrary.com/cn/vl_star.php?s=ae6qg
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=gvg
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=l4ea
                            http://www.javlibrary.com/cn/vl_star.php?s=ayba6
                            http://www.javlibrary.com/cn/vl_star.php?s=aerck
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=azobk
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=tki
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=aenq
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=RCTD
                            http://www.javlibrary.com/cn/vl_star.php?s=pe5a
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=dvdms
                            http://www.javlibrary.com/cn/vl_star.php?s=aerrg
                            http://www.javlibrary.com/cn/vl_star.php?s=afjdi
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=CJOD
                            http://www.javlibrary.com/cn/vl_star.php?s=pmmq
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=ddob
                            http://www.javlibrary.com/cn/vl_star.php?s=azgcg
                            http://www.javlibrary.com/cn/vl_star.php?s=aedsa
                            http://www.javlibrary.com/cn/vl_star.php?s=azhuy
                            http://www.javlibrary.com/cn/vl_star.php?s=kqea
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=WANZ
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=piwa
                            http://www.javlibrary.com/cn/vl_star.php?s=pivq
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=sora
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=OPUD
                            http://www.javlibrary.com/cn/vl_star.php?s=priq
                            http://www.javlibrary.com/cn/vl_star.php?s=aegas
                            http://www.javlibrary.com/cn/vl_star.php?s=kq5q
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=DOKS
                            http://www.javlibrary.com/cn/vl_star.php?s=ay2vc
                            http://www.javlibrary.com/cn/vl_star.php?s=azjsc
                            http://www.javlibrary.com/cn/vl_star.php?s=aebvs
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=BBAN
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=sdde
                            http://www.javlibrary.com/cn/vl_star.php?s=aezri
                            http://www.javlibrary.com/cn/vl_star.php?s=lbma
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=DPMI
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=SMSD
                            http://www.javlibrary.com/cn/vl_star.php?s=aebvk
                            http://www.javlibrary.com/cn/vl_star.php?s=afofm
                            http://www.javlibrary.com/cn/vl_star.php?s=aeidy
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=GTJ
                            http://www.javlibrary.com/cn/vl_star.php?s=ayad4
                            http://www.javlibrary.com/cn/vl_star.php?s=lvlq
                            http://www.javlibrary.com/cn/vl_star.php?s=ae3so
                            http://www.javlibrary.com/cn/vl_star.php?s=azoda
                            http://www.javlibrary.com/cn/vl_star.php?s=aenam
                            http://www.javlibrary.com/cn/vl_star.php?s=iyda
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=SVDVD
                            http://www.javlibrary.com/cn/vl_star.php?s=pjjq
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=azmbm
                            http://www.javlibrary.com/cn/vl_star.php?s=aedcy
                            http://www.javlibrary.com/cn/vl_star.php?s=onaa
                            http://www.javlibrary.com/cn/vl_star.php?s=aznf2
                            http://www.javlibrary.com/cn/vl_star.php?s=aygdk
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=ayudc
                            http://www.javlibrary.com/cn/vl_star.php?s=aelqc
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=AVSA
                            http://www.javlibrary.com/cn/vl_star.php?s=krfq
                            http://www.javlibrary.com/cn/vl_star.php?s=aeltg
                            http://www.javlibrary.com/cn/vl_star.php?s=area
                            http://www.javlibrary.com/cn/vl_star.php?s=aexcs
                            http://www.javlibrary.com/cn/vl_star.php?s=aeuv4
                            http://www.javlibrary.com/cn/vl_star.php?s=aensc
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=ayedw
                            http://www.javlibrary.com/cn/vl_star.php?s=afprk
                            http://www.javlibrary.com/cn/vl_star.php?s=aepqm
                            http://www.javlibrary.com/cn/vl_star.php?s=l5ca
                            http://www.javlibrary.com/cn/vl_star.php?s=aeyay
                            http://www.javlibrary.com/cn/vl_star.php?s=ae3c6
                            http://www.javlibrary.com/cn/vl_star.php?s=aenfs
                            http://www.javlibrary.com/cn/vl_star.php?s=ay7ei
                            http://www.javlibrary.com/cn/vl_star.php?s=aede4
                            http://www.javlibrary.com/cn/vl_star.php?s=aenew
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=SQTE
                            http://www.javlibrary.com/cn/vl_star.php?s=aelqo
                            http://www.javlibrary.com/cn/vl_star.php?s=aegbi
                            http://www.javlibrary.com/cn/vl_star.php?s=ae2be
                            http://www.javlibrary.com/cn/vl_star.php?s=azaqs
                            http://www.javlibrary.com/cn/vl_star.php?s=aejcy
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=mvg
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=cmc
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=cmv
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=fset&page=45
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=enki
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=pfaq
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=cetd
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=cmn
                            http://www.javlibrary.com/cn/vl_star.php?s=aevra
                            http://www.javlibrary.com/cn/vl_star.php?s=aydbm
                            http://www.javlibrary.com/cn/vl_star.php?s=ay2re
                            http://www.javlibrary.com/cn/vl_star.php?s=ae7cm
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=hjmo
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=aezf2
                            http://www.javlibrary.com/cn/vl_star.php?s=ae3d4
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=aejfi
                            http://www.javlibrary.com/cn/vl_star.php?s=brkq
                            http://www.javlibrary.com/cn/vl_star.php?s=pjjq
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=a4waa
                            http://www.javlibrary.com/cn/vl_star.php?s=aedco
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=HUNBL
                            http://www.javlibrary.com/cn/vl_star.php?s=ay6so
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=armd
                            http://www.javlibrary.com/cn/vl_star.php?s=aervm
                            http://www.javlibrary.com/cn/vl_star.php?s=afous
                            http://www.javlibrary.com/cn/vl_star.php?s=ae4e2
                            http://www.javlibrary.com/cn/vl_star.php?s=aeks6
                            http://www.javlibrary.com/cn/vl_star.php?s=aebcg
                            http://www.javlibrary.com/cn/vl_genre.php?g=aafa
                            http://www.javlibrary.com/cn/vl_director.php?d=aigq
                            http://www.javlibrary.com/cn/vl_star.php?s=aybuw
                            http://www.javlibrary.com/cn/vl_star.php?s=aehq6
                            http://www.javlibrary.com/cn/vl_star.php?s=afprg
                            http://www.javlibrary.com/cn/vl_star.php?s=aygqk
                            http://www.javlibrary.com/cn/vl_star.php?s=aydui
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=gvh
                            http://www.javlibrary.com/cn/vl_star.php?s=aerek
                            http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=PFES
                            http://www.javlibrary.com/cn/vl_star.php?s=afido
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqia
                            http://www.javlibrary.com/cn/vl_genre.php?g=argq
                            http://www.javlibrary.com/cn/vl_genre.php?g=ay
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqda
                            http://www.javlibrary.com/cn/vl_genre.php?g=a4wa
                            http://www.javlibrary.com/cn/vl_genre.php?g=ca
                            http://www.javlibrary.com/cn/vl_genre.php?g=jq
                            http://www.javlibrary.com/cn/vl_genre.php?g=armq
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqvq
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqaq
                            http://www.javlibrary.com/cn/vl_genre.php?g=du
                            http://www.javlibrary.com/cn/vl_genre.php?g=a5cq
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqaa
                            http://www.javlibrary.com/cn/vl_genre.php?g=i4
                            http://www.javlibrary.com/cn/vl_genre.php?g=arca
                            http://www.javlibrary.com/cn/vl_genre.php?g=iq
                            http://www.javlibrary.com/cn/vl_genre.php?g=arba
                            http://www.javlibrary.com/cn/vl_genre.php?g=pq
                            http://www.javlibrary.com/cn/vl_genre.php?g=arbq
                            http://www.javlibrary.com/cn/vl_genre.php?g=ke
                            http://www.javlibrary.com/cn/vl_genre.php?g=mi
                            http://www.javlibrary.com/cn/vl_genre.php?g=l4
                            http://www.javlibrary.com/cn/vl_genre.php?g=am
                            http://www.javlibrary.com/cn/vl_genre.php?g=ce
                            http://www.javlibrary.com/cn/vl_genre.php?g=aq7q
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqma
                            http://www.javlibrary.com/cn/vl_genre.php?g=arma
                            http://www.javlibrary.com/cn/vl_genre.php?g=py
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqoq
                            http://www.javlibrary.com/cn/vl_genre.php?g=ardq
                            http://www.javlibrary.com/cn/vl_genre.php?g=cm
                            http://www.javlibrary.com/cn/vl_genre.php?g=ou
                            http://www.javlibrary.com/cn/vl_genre.php?g=oa
                            http://www.javlibrary.com/cn/vl_genre.php?g=b4
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqnq
                            http://www.javlibrary.com/cn/vl_genre.php?g=ae
                            http://www.javlibrary.com/cn/vl_genre.php?g=aq7a
                            http://www.javlibrary.com/cn/vl_genre.php?g=iu
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqdq
                            http://www.javlibrary.com/cn/vl_genre.php?g=la
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqba
                            http://www.javlibrary.com/cn/vl_genre.php?g=araq
                            http://www.javlibrary.com/cn/vl_genre.php?g=bi
                            http://www.javlibrary.com/cn/vl_genre.php?g=n4
                            http://www.javlibrary.com/cn/vl_genre.php?g=arpq
                            http://www.javlibrary.com/cn/vl_genre.php?g=ki
                            http://www.javlibrary.com/cn/vl_genre.php?g=aqwq
                            http://www.javlibrary.com/cn/vl_genre.php?g=lm
                            http://www.javlibrary.com/cn/vl_genre.php?g=je
                            http://www.javlibrary.com/cn/vl_searchbyid.php?&keyword=ECB
                            http://www.javlibrary.com/cn/vl_genre.php?g=ampq
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=aehv2
                            http://www.javlibrary.com/cn/vl_star.php?&mode=&s=ae4ra&page=2
                            http://www.javlibrary.com/cn/vl_star.php?s=kamq
                            http://www.javlibrary.com/cn/vl_star.php?s=ayxtu
                            http://www.javlibrary.com/cn/vl_star.php?s=aexq";

            var list = input.Split('\n');

            foreach (var l in list)
            {
                var temp = l.Trim();

                var res = MagnetUrlService.GetFaviUrl(temp).Result;

                MagnetUrlService.SaveFaviUrl(res).Wait();
            }

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