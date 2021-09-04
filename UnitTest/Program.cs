using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DAL;
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

            Console.ReadKey();
        }

        private static void PrintLog(object sender, string e)
        {
            Console.WriteLine(e);
        }
    }

    public class ScanParam
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
    }
}