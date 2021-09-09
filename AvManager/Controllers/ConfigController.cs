using DAL;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigController : Controller
    {
        [HttpGet]
        public async Task<Settings> GetConfig()
        {
            return await new SettingsDAL().GetAllSettings();
        }
    }
}
