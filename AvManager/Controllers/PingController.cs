using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvManager.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Ping()
        {
            return "Pong";
        }
    }
}
