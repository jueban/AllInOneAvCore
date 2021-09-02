using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        [HttpPost]
        public JsonResult PotPlayerPlay([FromBody] List<string> files)
        {
            var key = Guid.NewGuid().ToString();

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);

                SettingService.InsertPlayHistory(new PlayHistory()
                {
                    FileName = fi.Name,
                    PlayTimes = 1,
                    SetNotPlayed = false
                });
            }

            RedisService.SetHashAndReplace("play", key, JsonConvert.SerializeObject(files));

            using (HttpClient client = new())
            {
                client.GetAsync($"http://localhost:20002/job/GeneratePotPlayerListAndPlay?key={key}").Wait();
            }

            return Json(new { success = true });
        }
    }
}
