using AvManager.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : Controller
    {
        [HttpGet]
        public JsonResult Index()
        {
            string text = "haha";

            return Json(new { status = true, msg = text });
        }

        [HttpGet]
        public JsonResult OpenExe()
        {
            Process.Start(Setting.POTPLAYEREXEFILELOCATION);

            return Json(new { status = true, msg = "" });
        }
    }
}
