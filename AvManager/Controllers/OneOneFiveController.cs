using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OneOneFiveController : Controller
    {
        [HttpPost]
        public async Task<ApiViewModel> SaveOneOneFiveCookie(string cookie, string userAgent)
        {
            ApiViewModel ret = new ApiViewModel();

            try
            {
                await OneOneFiveService.SaveOneOneFiveCookie(cookie, userAgent);
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
