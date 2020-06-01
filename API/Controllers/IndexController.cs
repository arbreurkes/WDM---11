using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello, this is the endpoint for the WDM project.";
        }


    }
}