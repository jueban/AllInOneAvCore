using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        [Route("PostSeedFiles")]
        public async Task<string> PostSeedFiles([FromForm(Name = "file")] List<IFormFile> files)
        {
            LogHelper.Info($"接受上传文件的个数{files.Count}");
            return await PostFiles(files, "c:\\FileUpload\\Seeds\\", false, ".torrent");
        }

        #region 工具
        private async Task<string> PostFiles(List<IFormFile> filelist, string folder, bool addDate, string ext)
        {
            StringBuilder sb = new StringBuilder();

            if (filelist != null && filelist.Count > 0)
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    IFormFile file = filelist[i];
                    string fileName = file.FileName;

                    if (fileName.ToLower().Contains(ext))
                    {
                        if (addDate)
                        {
                            folder = folder + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                        }

                        DirectoryInfo di = new DirectoryInfo(folder);

                        if (!di.Exists)
                        {
                            di.Create();
                        }

                        var filePath = folder + fileName;

                        if (file.Length > 0)
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                    }
                    else
                    {
                        sb.AppendLine("传入格式不正确: " + fileName);
                    }
                }
            }
            else
            {
                sb.AppendLine("上传的文件信息不存在！");
            }

            return sb.ToString();
        }
        #endregion
    }
}
