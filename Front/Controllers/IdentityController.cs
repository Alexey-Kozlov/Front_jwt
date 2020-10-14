using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class IdentityController : Controller
    {
        [HttpPost]
        public IActionResult IndexPost()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
