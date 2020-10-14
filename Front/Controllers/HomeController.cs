using System;
using System.Collections.Generic;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Front.Services;

namespace Front.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestService testService;
        public HomeController(ITestService _testService)
        {
            testService = _testService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Service()
        {

            //HttpClient httpClient2 = new HttpClient();
            //var dd = User.Identity;
            //var ss1 =  HttpContext.GetTokenAsync("access_token");
            //httpClient2.SetBearerToken(ss1.Result);

            //var response = Task.Run(async () => await httpClient2.GetAsync("https://ws-pc-70:5007/identity"));
            //var ss = response.Result;
            //var ww = Task.Run(async () => await ss.Content.ReadAsStringAsync());
            //var kk = ww.Result;
            ViewData["Test"] = await testService.TestWebApi(HttpContext);
            return View();

        }



        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
