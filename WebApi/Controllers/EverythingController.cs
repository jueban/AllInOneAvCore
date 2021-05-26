using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utils;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EverythingController : ControllerBase
    {
        [HttpGet]
        public async Task<EverythingResult> EverythingSearch(string content)
        {
            var retModel = new EverythingResult();

            retModel = await EverythingService.EverythingSearch(content);

            if (retModel == null || retModel.results == null || retModel.results.Count <= 0)
            {          
                retModel = new EverythingResult
                {
                    results = new List<EverythingFileResult>()
                };

                List<OneOneFiveFileItemModel> oneOneFiveFiles = await OneOneFiveService.Get115SearchFileResult(content, OneOneFiveFolder.AV, true);

                if (oneOneFiveFiles != null && oneOneFiveFiles.Any())
                {
                    retModel.totalResults = oneOneFiveFiles.Count + "";

                    foreach (var file in oneOneFiveFiles)
                    {
                        EverythingFileResult temp = new()
                        {
                            size = file.s + "",
                            sizeStr = FileUtility.GetAutoSizeString(double.Parse(file.s + ""), 1),
                            location = "115网盘",
                            name = file.n,
                            path = file.pc,
                            type = file.@class
                        };

                        retModel.results.Add(temp);
                    }
                }
            }

            return retModel;
        }
    }
}
