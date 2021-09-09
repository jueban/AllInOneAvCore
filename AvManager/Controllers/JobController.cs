using AvManager.Hangfire;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvManager.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JobController : Controller
    {
        [HttpGet]
        public string OpenBroswer()
        {
            BackgroundJob.Enqueue(() => Jobs.OpenBroswerJob());

            return "success";
        }

        [HttpGet]
        public string ScanJavLibraryAllUrlsAndSave()
        {
            BackgroundJob.Enqueue(() => Jobs.ScanJavLibraryAllUrlsAndSave());

            return "success";
        }

        [HttpGet]
        public string ScanUrlsOccursError(string file)
        {
            BackgroundJob.Enqueue(() => Jobs.ScanUrlsOccursError(file));

            return "success";
        }

        [HttpGet]
        public string ScanAllNotDownloadJavLibraryUrls()
        {
            BackgroundJob.Enqueue(() => Jobs.ScanAllNotDownloadJavLibraryUrls());

            return "success";
        }

        [HttpGet]
        public string ScanJavLibraryUpdateUrls(JavLibraryEntryPointType entry, int pages, string url, bool useExactPassin)
        {
            BackgroundJob.Enqueue(() => Jobs.ScanJavLibraryUpdateUrls(entry, pages, url, useExactPassin));

            return "success";
        }

        [HttpGet]
        public string ScanJavUpdate(string site, int page = 0)
        {
            BackgroundJob.Enqueue(() => Jobs.ScanJavUpdate(site, page));

            return "success";
        }

        [HttpGet]
        public string GenerateReport()
        {
            BackgroundJob.Enqueue(() => Jobs.GenerateReport());

            return "success";
        }

        [HttpGet]
        public string RegJavlibraryDailyUpdate()
        {
            RecurringJob.AddOrUpdate(() => Jobs.ScanJavLibraryUpdateUrls(JavLibraryEntryPointType.Update, 200, "", false), Cron.HourInterval(4));

            return "success";
        }

        [HttpGet]
        public string PingService()
        {
            RecurringJob.AddOrUpdate(() => Jobs.PingService(), Cron.MinuteInterval(30));

            return "success";
        }

        [HttpGet]
        public string GeneratePotPlayerListAndPlay(string key)
        {
            BackgroundJob.Enqueue(() => Jobs.GeneratePotPlayerListAndPlay(key));

            return "success";
        }
    }
}
