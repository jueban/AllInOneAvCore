using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utils;

namespace Hangfire.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JobController : ControllerBase
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
    }
}
