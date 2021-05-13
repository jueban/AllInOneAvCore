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
    public class Helper
    {
        public async static Task<int> DownloadJavLibraryDetailAndSavePicture(List<WebScanUrlModel> waitForDownload)
        {
            int ret = 0;

            var random = new Random();

            var imageFolder = SettingService.GetSetting().Result.JavLibraryImageFolder;

            if (!Directory.Exists(imageFolder))
            {
                Directory.CreateDirectory(imageFolder);
            }

            await Task.Run(() => Parallel.ForEach(waitForDownload, new ParallelOptions { MaxDegreeOfParallelism = 10 }, toBeDownload =>
            {
                var avModelScan = JavLibraryService.GetJavLibraryDetailPageInfo(toBeDownload.URL).Result;

                if (avModelScan.exception == null && avModelScan.avModel != null)
                {
                    var result = 0;
                    try
                    {
                        result = JavLibraryService.SaveJavLibraryAvModel(avModelScan.avModel).Result;
                    }
                    catch (Exception)
                    {
                        LogHelper.Info($"<=====获取 {toBeDownload.URL} 失败=====>");
                    }

                    JavLibraryService.SaveCommonJavLibraryModel(avModelScan.infos).Wait();

                    if (result > 0)
                    {
                        ret++;
                        JavLibraryService.UpdateJavLibraryScanDownloadState(toBeDownload.Id, true).Wait();
                    }

                    var picFile = imageFolder + "\\" + avModelScan.avModel.FileNameWithoutExtension + ".jpg";

                    if (!string.IsNullOrWhiteSpace(avModelScan.avModel.PicUrl) && !File.Exists(picFile))
                    {
                        try
                        {
                            new System.Net.WebClient().DownloadFile(avModelScan.avModel.PicUrl, picFile);
                        }
                        catch (Exception)
                        {
                            LogHelper.Info($"<=====下载图片 {avModelScan.avModel.PicUrl} 失败=====>");
                        }
                    }
                }
                else
                {
                    LogHelper.Info($"<=====获取 {toBeDownload.URL} 失败=====>");
                }

                Task.Delay(random.Next(50)).Wait();
            }));

            return ret;
        }

        public async static Task<List<WebScanUrlModel>> GetJavLibraryWebScanUrlMode(JavLibraryEntryPointType entry, int pages, string url, bool useExactPassin)
        {
            List<WebScanUrlModel> scans = new();

            var firstPageResult = JavLibraryService.GetJavLibraryListPageInfo(entry, url, 1, useExactPassin).Result;
            var totalPage = firstPageResult.pageCount;

            List<int> pageRange = new();

            for (int i = 1; i <= pages && i <= totalPage; i++)
            {
                pageRange.Add(i);
            }

            await Task.Run(() => Parallel.ForEach(pageRange, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
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
            }));

            return scans;
        }
    }
}
