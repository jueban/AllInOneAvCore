using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hangfire.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JobController : ControllerBase
    {
        [HttpGet]
        public string OpenBroswer(string url)
        {
            BackgroundJob.Enqueue(() => OpenBroswerJob());

            return "success";
        }

        public void OpenBroswerJob()
        {
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "https://www.baidu.com");
        }
    }
}
