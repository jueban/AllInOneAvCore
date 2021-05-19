using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace WebMVC.Controllers
{
    public class LocalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetRoots()
        {
            var ret = LocalService.GetSystemTreeView();

            return Json(ret);
        }

        public JsonResult GetFilesAndFolder(string root)
        {
            var ret = LocalService.GetFilesAndFolders(root);

            return Json(ret);
        }
    }
}
