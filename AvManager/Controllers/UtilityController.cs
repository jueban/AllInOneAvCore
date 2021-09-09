using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvManager.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilityController : Controller
    {
        public RecordCarPlateModel RecordCarPlate(string plate, string reason)
        {
            var ret = UtilityService.RecordCarPlate(plate, reason);

            return new RecordCarPlateModel() { Ret = ret };
        }
    }

    public class RecordCarPlateModel
    {
        public string Ret { get; set; }
    }
}
