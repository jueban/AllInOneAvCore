using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
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

        public IActionResult ScanResult()
        {
            ViewData.Add("Title", "扫描结果-扫描");
            return View();
        }

        public IActionResult ShowScanResult(int id)
        {
            ViewData.Add("Title", "扫描详情-扫描");
            return View();
        }

        public ScanPageModel GetJavLibraryData()
        {
            var ret = MagnetUrlService.GetScanPageMode(WebScanUrlSite.JavLibrary);

            return ret;
        }

        public List<ScanResult> GetScanResult()
        {
            var ret = new ScanDAL().GetSeedMagnetSearchModelAll().Result;

            return ret;
        }
    }
}
