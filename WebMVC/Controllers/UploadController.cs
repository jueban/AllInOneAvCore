using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}
