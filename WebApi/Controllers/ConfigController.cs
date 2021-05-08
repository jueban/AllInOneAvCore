using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        public readonly IWebHostEnvironment _webHostEnvironment;

        public ConfigController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public ConfigModel GetConfigModel(string site, string key)
        {
            ConfigModel ret = new ConfigModel();
            var folder = _webHostEnvironment.ContentRootPath;
            var settingFile = folder + "\\setting.json";

            if (System.IO.File.Exists(settingFile))
            {
                var content = new StreamReader(settingFile).ReadToEnd();
                var json = JObject.Parse(content);
                if (json.ContainsKey(site))
                {
                    if (((JObject)json[site]).ContainsKey(key))
                    {
                        ret.status = ApiViewModelStatus.Success;
                        ret.Value = ((JObject)json[site])[key].ToString();
                    }
                    {
                        ret.status = ApiViewModelStatus.NotExists;
                    }
                }
                else
                {
                    ret.status = ApiViewModelStatus.NotExists;
                }
            }
            else
            {
                ret.status = ApiViewModelStatus.NotExists;
            }

            return ret;
        }
    }
}
