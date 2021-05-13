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
        //增加[FromForm(Name = "file")] 或者去掉 [ApiController]
        [HttpPost]
        [Route("PostSeedFiles")]
        public async Task<string> PostSeedFiles([FromForm(Name = "file")] List<IFormFile> files)
        {
            return await PostFiles(files, @"C:\FileUpload\Seeds\", false, ".torrent");
        }

        #region 工具
        private static async Task<string> PostFiles(List<IFormFile> filelist, string folder, bool addDate, string ext)
        {
            StringBuilder sb = new();

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

                        DirectoryInfo di = new(folder);

                        if (!di.Exists)
                        {
                            di.Create();
                        }

                        var filePath = folder + fileName;

                        if (file.Length > 0)
                        {
                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);
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
