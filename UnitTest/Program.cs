using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using Hangfire;
using JobHub.Hubs;
using Models;
using Newtonsoft.Json;
using Services;
using Utils;

namespace UnitTest
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            Progress<string> progress = new();
            progress.ProgressChanged += PrintLog;

            //var rest = OneOneFiveService.GetM3U8("acjpxvomv83tlhtnb").Result;

            //System.Diagnostics.Process.Start("C:\\Program Files\\DAUM\\PotPlayer\\PotPlayerMini64.exe", rest);

            //OneOneFiveService.UpdateKeepAvs().Wait();

            //OneOneFiveService.Get115Cookie().Wait();
            //var files = OneOneFiveService.GetSameAvNameFiles().Result;
            //OneOneFiveService.DeleteSameAvNameFiles(files, progress).Wait();

            //LocalService.Rename(@"N:\new", progress).Wait();

            //MagnetUrlService.SearchJavBus("https://www.javbus.com/page", 5, "Test", progress).Wait();

            //var av = new JavLibraryDAL().GetAvModelByWhere("").Result;

            //RedisService.SetHashAndReplace("play", "", "");

            //Process.Start(@"‪C:\Users\cleus\AppData\Local\115Chrome\Application\115chrome.exe");

            //Jobs.ScanJavLibraryUpdateUrls(JavLibraryEntryPointType.Update, 200, "", false);

            //var ret = RedisService.GetInfo("videoTemp");

            Console.ReadKey();
        }

        private static void PrintLog(object sender, string e)
        {
            Console.WriteLine(e);
        }

        private static void Test()
        { 
            var filePaht = @"O:\downloads\";

            var files = new DirectoryInfo(filePaht).GetFiles("*.*", SearchOption.AllDirectories);

            files = files.Where(x => x.Length > 1024 * 1024 * 300).ToArray();

            var dic = files.GroupBy(x => x.Length).ToDictionary(x => x.Key, x => x.ToList());

            dic = dic.Where(x => x.Value.Count > 1).ToDictionary(x => x.Key, x => x.Value);

            int delete = 0;

            foreach(var d in dic)
            {
                var deleteOne = d.Value.OrderByDescending(x => x.FullName.Length).FirstOrDefault();

                if (deleteOne.FullName.Contains("(1)") || deleteOne.FullName.Contains("(2)"))
                {
                    File.Delete(deleteOne.FullName);

                    delete++;
                }
            }

            Console.WriteLine(delete);
        }
    }

    public class ScanParam
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
    }
}