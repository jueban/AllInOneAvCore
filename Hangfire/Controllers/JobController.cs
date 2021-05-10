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
        public string OpenBroswer(string location, string url)
        {
            BackgroundJob.Enqueue(() => OpenBroswerJob(location, url));

            return "success";
        }

        public void OpenBroswerJob(string location, string url)
        {
            var process = System.Diagnostics.Process.Start(location, url);
        }

        //没想好怎么搞processId
        public void CloseBroswerJob(int processId)
        {
            var process = System.Diagnostics.Process.GetProcessById(processId);

            if (process != null && process.HasExited != true)
            {
                process.Kill();
                process.Dispose();
            }
        }
    }
}
