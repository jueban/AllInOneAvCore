using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
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
    }
}
