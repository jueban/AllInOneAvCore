using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    [Authorize]
    public class PlayController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PlayLocal(string file)
        {
            ViewData.Add("Title", "播放-" + file);
            ViewData.Add("Video", file);
            return View();
        }

        public IActionResult PlayLocalVideo(string file)
        {
            return PhysicalFile(file, "application/octet-stream", enableRangeProcessing: true);
        }

        public IActionResult PlayLocalMultiple(string key)
        {
            ViewData.Add("Title", "播放-本地列表");          

            return View();
        }

        public JsonResult GetLocalMultiplePlayAvs(string key)
        {
            var avs = JsonConvert.DeserializeObject<List<MyFileInfo>>(RedisService.GetHash("play", key));

            return Json(new { success = true, data = avs });
        }
    }
}
