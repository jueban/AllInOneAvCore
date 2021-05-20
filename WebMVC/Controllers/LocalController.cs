using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

        public IActionResult Rename(string folder)
        {
            ViewData.Add("Title", "本地-重命名");
            ViewData.Add("Folder", folder);
            ViewData.Add("Infos", LocalService.GetFolderInfo(folder));

            return View();
        }

        public IActionResult ManualRename(string folder)
        {
            ViewData.Add("Title", "本地-手动重命名");
            ViewData.Add("Folder", folder);

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

        public JsonResult GetFilesInFolder(string folder)
        {
            return Json(LocalService.GetFolderFiles(folder));
        }

        [HttpPost]
        public JsonResult GetPossibleAvNameAndInfo([FromBody]string fileName)
        {
            var ret = LocalService.GetPossibleAvNameAndInfo(fileName);

            return Json(ret);
        }
    }
}
