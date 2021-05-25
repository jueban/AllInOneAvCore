using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace WebMVC.Controllers
{
    public class ScanController : Controller
    {
        public IActionResult Index()
        {
            ViewData.Add("Title", "设置-扫描");
            return View();
        }

        [HttpGet]
        public async Task<WebResult> SaveFaviUrl(string url)
        {
            var res = MagnetUrlService.GetFaviUrl(url);

            await MagnetUrlService.SaveFaviUrl(res);

            return new WebResult()
            {
                msg = res.url,
                status = Status.Ok
            };
        }

        public IActionResult ScanJavLibrary() 
        {
            ViewData.Add("Title", "JavLibrary-扫描");
            return View();
        }

        public IActionResult ScanJavBus()
        {
            ViewData.Add("Title", "JavBus-扫描");
            return View();
        }

        public ScanPageModel GetJavLibraryData()
        {
            ScanPageModel ret = new()
            {
                Drops = new List<ScanPageDrop>(),
                Name = "WebScan",
                Page = 9999,
                Url = ""
            };

            ret.Drops.Add(new ScanPageDrop()
            {
                Title = "选择页面",
                Items = new List<ScanPageDropItem>() { 
                    new ScanPageDropItem() { 
                        Text = "更新",
                        Value = "http://www.javlibrary.com/cn/vl_update.php?&mode="
                    },
                    new ScanPageDropItem(){ 
                        Text = "新加入",
                        Value = "http://www.javlibrary.com/cn/vl_newentries.php?&mode="
                    },
                    new ScanPageDropItem(){
                        Text = "最想要",
                        Value = "http://www.javlibrary.com/cn/vl_mostwanted.php?&mode="
                    },
                    new ScanPageDropItem(){
                        Text = "高评价",
                        Value = "http://www.javlibrary.com/cn/vl_bestrated.php?&mode="
                    }
                }
            });

            return ret;
        }
    }
}
