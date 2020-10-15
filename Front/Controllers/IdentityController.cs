using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Front.Services;

namespace Front.Controllers
{
    [Route("[controller]")]
    public class IdentityController : Controller
    {
        private readonly ITestService testService;

        public IdentityController(ITestService _testService)
        {
            testService = _testService;
        }

        [HttpPost]
        public IActionResult IndexPost()
        {
            return View();
        }

        [HttpGet("User")]
        public async Task<IActionResult> IndexUser()
        {
            ViewData["Test"] = await testService.TestWebApi(HttpContext, "identity/user");
            return View("Service");
        }

        [HttpGet("Admin")]
        public async Task<IActionResult> IndexAdmin()
        {
            ViewData["Test"] = await testService.TestWebApi(HttpContext, "identity/admin");
            return View("Service");
        }
    }
}
