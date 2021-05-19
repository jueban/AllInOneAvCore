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
            ViewData.Add("Title", "本地-浏览");
            return View();
        }

        public IActionResult RemoveFolder(string folder)
        {
            ViewData.Add("Title", "本地-去子文件夹");
            ViewData.Add("Folder", folder);
            ViewData.Add("Infos", LocalService.GetFolderInfo(folder));

            return View();
        }

        public IActionResult Rename()
        {
            ViewData.Add("Title", "本地-重命名");
            return View();
        }

        public IActionResult Move()
        {
            ViewData.Add("Title", "本地-移动");
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
