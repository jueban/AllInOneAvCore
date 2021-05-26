using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace WebMVC.Controllers
{
    public class ScanController : Controller
    {
        public IActionResult Index()
        {
            ViewData.Add("Title", "设置-扫描");
            return View();
        }

        [HttpGet]
        public async Task<WebResult> SaveFaviUrl(string url)
        {
            var res = await MagnetUrlService.GetFaviUrl(url);

            await MagnetUrlService.SaveFaviUrl(res);

            return new WebResult()
            {
                msg = res.url,
                status = Status.Ok
            };
        }

        public IActionResult ScanJavLibrary() 
        {
            ViewData.Add("Title", "JavLibrary-扫描");
            return View();
        }

        public IActionResult ScanJavBus()
        {
            ViewData.Add("Title", "JavBus-扫描");
            return View();
        }

        public ScanPageModel GetJavLibraryData()
        {
            var ret = MagnetUrlService.GetScanPageMode(WebScanUrlSite.JavLibrary);

            return ret;
        }
    }
}
