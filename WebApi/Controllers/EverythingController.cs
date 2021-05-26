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
            var retModel = await EverythingService.SearchBothLocalAnd115LocalFirst(content);

            return retModel;
        }
    }
}