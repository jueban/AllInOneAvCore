using System;
using System.Collections.Generic;
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
            Progress<string> progress = new Progress<string>();
            progress.ProgressChanged += PrintLog;

            //var rest = OneOneFiveService.GetM3U8("acjpxvomv83tlhtnb").Result;

            //OneOneFiveService.UpdateKeepAvs().Wait();

            //var files = OneOneFiveService.GetRepeatFiles().Result;
            //OneOneFiveService.DeleteSameAvNameFiles(files, progress).Wait();

            OneOneFiveService.CheckPossibleDuplicatedAv(OneOneFiveFolder.Fin).Wait();

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