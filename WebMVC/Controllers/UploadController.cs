using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "上传文件";
            ViewData.Add("api", SettingService.GetSetting().Result.ApiSite);

            return View();
        }
    }
}
