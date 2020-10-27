using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Front_jwt.Services;

namespace Front_jwt.Controllers
{
    [Route("[controller]")]
    public class IdentityController : Controller
    {

        public IdentityController()
        {
        }

        [HttpPost]
        public IActionResult IndexPost()
        {
            return View();
        }

    }
}
