using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class JavLibraryApiController : ControllerBase
    {
        private readonly ILogger<JavLibraryApiController> _logger;

        public JavLibraryApiController(ILogger<JavLibraryApiController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ApiViewModel> SaveJavlibraryCookie(string cookie, string userAgent)
        {
            ApiViewModel ret = new ApiViewModel();         
            
            try
            {
                await JavLibraryService.SaveJavLibraryCookie(cookie, userAgent);
            }
            catch(Exception ee)
            {
                ret.status = ApiViewModelStatus.Exception;
                ret.msg = ee.ToString();
            }

            return ret;
        }
    }
}
