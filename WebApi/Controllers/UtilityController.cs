using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilityController : ControllerBase
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
