using System;
using System.Collections.Generic;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Front_jwt.Services;
using IdentityModel;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Front_jwt.Models;
using Microsoft.AspNetCore.Authorization;

namespace Front_jwt.Controllers
{
    public class HomeController : Controller
    {
        private readonly CertificatesSettings _certificatesSettings;
        private readonly ServiceUrlsSettings _serviceUrlsSettings;
        //private readonly IIdentityServerClient _identityServerClient; //<- этот сервис нужен, если мы работает через IIdentityServerClient,
        //а не через IAddHeaderClient - в нем уже добавлены заголовки, чтоб не делать все время одну и ту же работу
        private readonly IAddHeaderClient _addHeaderClient;

        public HomeController(IOptions<CertificatesSettings> certificatesSettings, IOptions<ServiceUrlsSettings> serviceUrlsSettings,
           // IIdentityServerClient identityServerClient, 
            IAddHeaderClient addHeaderClient)
        {
            _certificatesSettings = certificatesSettings.Value;
            _serviceUrlsSettings = serviceUrlsSettings.Value;
         // _identityServerClient = identityServerClient;
            _addHeaderClient = addHeaderClient;
        }

        //    //Здесь используется секция services.AddAuthentication().AddOpenIdConnect(options => ..... из startup
        //    //Использует при каждом вызове контроллера вызов ClientCredentialsTokenRequest из startup
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    HttpClient client = new HttpClient();
        //    client.SetBearerToken(await _identityServerClient.RequestClientCredentialsTokenAsync());
        //    var response2 = await client.GetAsync($"{_serviceUrlsSettings.WebApiEndpoint}/identity/test");
        //    ViewData["Test"] = await response2.Content.ReadAsStringAsync();
        //    return View("Index");
        //}

        //Здесь используются сервисы с автоматическим добавлением заголовков. Но запрос к identity идет при каждом обращении к контроллеру
        [HttpGet("test2")]
        public async Task<IActionResult> Index4()
        {
            var result = await _addHeaderClient.GetdataFromApi();
            ViewData["Test"] = result;
            return View("Index");
        }
            

        //здесь запрос через напрямую, без сервисов из startup. Их можно отключить в startup для этого случая, иначе они будут вызываться 2 раза
        //используются сессии для кеширования значения токена, чтоб исключить обращение к identity при каждом запросе.
        [HttpGet("test")]
        public async Task<IActionResult> Index3()
        {
            HttpClient httpClient = await SetSession();
            var response2 = await httpClient.GetAsync($"{_serviceUrlsSettings.WebApiEndpoint}/identity/test");
            ViewData["Test"] = await response2.Content.ReadAsStringAsync();
            return View("Index");

        }

        private async Task<HttpClient> SetSession()
        {
            //создание сессии с токеном доступа
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("token")))
            {
                HttpClient client = new HttpClient();
                var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = $"{_serviceUrlsSettings.AuthorityApiEndpoint}/connect/token",
                    GrantType = OidcConstants.GrantTypes.AuthorizationCode,
                    Scope = "api",
                    ClientAssertion = new ClientAssertion
                    {
                        Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                        Value = TokenGenerator.CreateClientAuthJwt(_certificatesSettings.Path, _certificatesSettings.Password,
                        _serviceUrlsSettings.AuthorityApiEndpoint)
                    }
                });
                HttpContext.Session.SetString("token", response.AccessToken);
            }
            HttpClient client2 = new HttpClient();
            client2.SetBearerToken(HttpContext.Session.GetString("token"));
            return client2;
        }
    }
}
