using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Services;

namespace WebMVC.Controllers
{
    [Route("[controller]/[action]")]
    public class WangpanController : Controller
    {
        public IActionResult Index()
        {
            ViewData.Add("Title", "网盘-首页");
            return View();
        }

        public WebResult ScanLocalToUpload()
        {
            WebResult ret = new();
            ret.msg = OneOneFiveService.GetNeedToUpload115Avs();

            return ret;
        }

        public async Task<WebResult> MoveBackToFin()
        {
            WebResult ret = new();
            await OneOneFiveService.MoveNeedToUpload115AvsBackToFin();

            return ret;
        }

        public async Task<WebResult> MoveToFin()
        {
            WebResult ret = new();
            await OneOneFiveService.MoveToFin();

            return ret;
        }

        public async Task<WebResult> Refresh115Redis()
        {
            WebResult ret = new();
            await OneOneFiveService.RefreshOneOneFiveFinFilesCache();

            return ret;
        }

        public IActionResult RemoveDuplicate()
        {
            ViewData.Add("Title", "网盘-去重");
            return View();
        }

        public async Task<Dictionary<string, List<OneOneFiveDuplicateFileRemoveItem>>> GetDuplicateAvFile()
        {
            var model = await OneOneFiveService.GetSameAvNameFiles();

            return model.data;
        }

        public async Task<string> Getm3u8(string pc)
        {
            return await OneOneFiveService.GetM3U8(pc);
        }

        [HttpPost]
        public async Task<WebResult> Delete([FromBody] Dictionary<string, List<OneOneFiveDuplicateFileRemoveItem>> entity)
        {
            await OneOneFiveService.DeleteDuplicatedFiles(entity);

            return new WebResult()
            {
                msg = "保存成功",
                status = Status.Ok
            };
        }

        public IActionResult ChooseKeepFrom115()
        {
            ViewData.Add("Title", "网盘-保留115到本地");

            return View();
        }

        public async Task<KeepModel> GetKeepFiles(int pageSize, int page, bool force, int sizeLimit = 2)
        {
            return await OneOneFiveService.GetKeepAv(pageSize, page, force, sizeLimit);
        }

        public async Task<string> SaveBack(string fid)
        {
            var ret = await OneOneFiveService.Copy(new List<string> { fid }, OneOneFiveFolder.MoveBackToLocal);

            if (ret.state == true)
            {
                return "success";
            }
            else
            {
                return "fail";
            }
        }

        public IActionResult DeleteLocal()
        {
            ViewData.Add("Title", "网盘-删除本地");

            return View();
        }

        public async Task<KeepModel> GetDeleteFiles(int pageSize)
        {
            return await OneOneFiveService.GetDeleteAv(pageSize);
        }

        [HttpPost]
        public async Task<string> DeleteAndKeep(string model)
        {
            var data = JsonConvert.DeserializeObject<KeepModel>(model);

            var ret = await LocalService.KeepAndDelete(data);

            if (ret == 1)
            {
                return "success";
            }
            else
            {
                return "fail";
            }
        }
    }
}
