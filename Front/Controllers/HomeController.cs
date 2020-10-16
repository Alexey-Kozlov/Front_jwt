using System;
using System.Collections.Generic;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Front.Services;
using IdentityModel;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace Front.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();
            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://ws-pc-70:5001/connect/token",
                GrantType = OidcConstants.GrantTypes.AuthorizationCode,
                Scope = "api",
                ClientAssertion = new ClientAssertion
                {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = TokenGenerator.CreateClientAuthJwt()
                }
            });
            ViewData["Test"] = response.AccessToken;
            HttpContext.Session.SetString("token", response.AccessToken);
            return View();
        }

        [HttpGet("test")]
        public async Task<IActionResult> Index2()
        {

            HttpClient httpClient = new HttpClient();
            httpClient.SetBearerToken(HttpContext.Session.GetString("token"));

            var response2 = await httpClient.GetAsync($"https://ws-pc-70:5007/identity/test");

            ViewData["Test"] = await response2.Content.ReadAsStringAsync();
            return View("Index");

        }



        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
