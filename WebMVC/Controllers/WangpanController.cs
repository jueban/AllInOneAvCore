using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace WebMVC.Controllers
{
    [Route("[controller]/[action]")]
    public class WangpanController : Controller
    {
        public IActionResult Index()
        {
            ViewData.Add("Title", "网盘-首页");
            return View();
        }

        public WebResult ScanLocalToUpload()
        {
            WebResult ret = new();
            ret.msg = OneOneFiveService.GetNeedToUpload115Avs();

            return ret;
        }

        public async Task<WebResult> MoveBackToFin()
        {
            WebResult ret = new();
            await OneOneFiveService.MoveNeedToUpload115AvsBackToFin();

            return ret;
        }

        public async Task<WebResult> MoveToFin()
        {
            WebResult ret = new();
            await OneOneFiveService.MoveToFin();

            return ret;
        }

        public async Task<WebResult> Refresh115Redis()
        {
            WebResult ret = new();
            await OneOneFiveService.RefreshOneOneFiveFinFilesCache();

            return ret;
        }
    }
}
