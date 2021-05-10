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
            BackgroundJob.Enqueue(() => Jobs.OpenBroswerJob(location, url));

            return "success";
        }
    }
}
