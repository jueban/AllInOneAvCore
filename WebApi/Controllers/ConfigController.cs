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
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        public readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOptions<Settings> _settings;

        public ConfigController(IWebHostEnvironment webHostEnvironment, IOptions<Settings> settings)
        {
            _webHostEnvironment = webHostEnvironment;
            _settings = settings;
        }

        [HttpGet]
        public Settings GetConfig()
        {
            return _settings.Value;
        }
    }
}
