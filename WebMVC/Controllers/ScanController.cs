using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Services;

namespace WebMVC.Controllers
{
    [Authorize]
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
            ViewData.Add("Id", id);
            return View();
        }

        public IActionResult DeleteScanResult(int id)
        {
            var ret = new ScanDAL().DeleteSeedMagnetSearchModelById(id).Result;

            return Redirect("ScanResult");
        }

        public ScanPageModel GetJavLibraryData()
        {
            var ret = MagnetUrlService.GetScanPageMode(WebScanUrlSite.JavLibrary);

            return ret;
        }

        public ScanPageModel GetJavBusData()
        {
            var ret = MagnetUrlService.GetScanPageMode(WebScanUrlSite.JavBus);

            return ret;
        }

        public List<ScanResult> GetScanResult()
        {
            var ret = new ScanDAL().GetSeedMagnetSearchModelAll().Result;

            return ret;
        }

        public async Task<List<ShowMagnetSearchResult>> GetScanResultDetail(int id)
        {
            var ret = await MagnetUrlService.GetScanResultDetail(id);

            return ret;
        }

        [HttpPost]
        public async Task<OneOneFiveResult> Add115Task(string mag)
        {
            return await OneOneFiveService.AddOneOneFiveTask(mag);
        }

        [HttpPost]
        public string PushAndGetRedisKey(string str)
        {
            var guid = Guid.NewGuid().ToString();

            RedisService.SetHash("scan", guid, str);

            return guid;
        }
    }
}
