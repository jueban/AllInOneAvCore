using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvManager.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class JavLibraryApiController : Controller
    {
        [HttpPost]
        public async Task<ApiViewModel> SaveJavlibraryCookie(string cookie, string userAgent)
        {
            ApiViewModel ret = new ApiViewModel();

            try
            {
                await JavLibraryService.SaveJavLibraryCookie(cookie, userAgent);
            }
            catch (Exception ee)
            {
                ret.status = ApiViewModelStatus.Exception;
                ret.msg = ee.ToString();
            }

            return ret;
        }
    }
}
