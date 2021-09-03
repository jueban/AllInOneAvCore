using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
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

        public IActionResult RemoveDuplicate()
        {
            ViewData.Add("Title", "本地-去重");
            return View();
        }

        public IActionResult Play(string folder)
        {
            ViewData.Add("Title", "本地-播放");
            ViewData.Add("Folder", folder);

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

        [HttpPost]
        public JsonResult GetPossibleAvMatch([FromBody] string fileName)
        {
            var ret = LocalService.GetPossibleAvMatch(fileName);

            return Json(ret);
        }

        [HttpPost]
        public async Task<JsonResult> GetJavLibrarySearchResult([FromBody] string content)
        {
            var ret = await LocalService.GetJavLibrarySearchResult(content);

            return Json(ret);
        }

        [HttpPost]
        public ManualRenameResultModel ManualRename([FromBody] ManualRenameModel model)
        {
            ManualRenameResultModel ret = new();

            var res = LocalService.ManualRename(model).Result;

            ret.status = res ? Status.Ok : Status.Error;

            return ret;
        }

        [HttpGet]
        public async Task<Dictionary<string, List<MyFileInfo>>> GetDuplicateAvFile()
        {
            var ret = await LocalService.GetDuplicateAvFile();

            return ret;
        }

        [HttpPost]
        public WebResult DeleteFile([FromBody] List<string> files)
        {
            WebResult ret = new();
            var res = LocalService.DeleteFiles(files);

            if (res == 0)
            {
                ret.status = Status.Ok;
                ret.msg = "成功";
            }
            else
            {
                ret.status = Status.Exception;
                ret.msg = "失败" + res + "个";
            }

            return ret;
        }

        public JsonResult GetLocalAvs(string folder)
        {
            var files = LocalService.GetLocalAvs(folder);

            return Json(new { rows = files, count = files.Count });
        }

        [HttpPost]
        public JsonResult PushRedis([FromBody] List<MyFileInfo> avs)
        {
            var key = Guid.NewGuid().ToString();

            RedisService.SetHashAndReplace("play", key, JsonConvert.SerializeObject(avs));

            return Json(new { success = true, data = key });
        }
    }
}
