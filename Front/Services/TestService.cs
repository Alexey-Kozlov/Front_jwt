using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Front.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace Front.Services
{
    public class TestService :ITestService
    {
        private readonly ServiceUrlsSettings _serviceUrlsSettings;
        public TestService(IOptions<ServiceUrlsSettings> serviceUrlsSettings)
        {
            _serviceUrlsSettings = serviceUrlsSettings.Value;
        }

        public async Task<string> TestWebApi(HttpContext context)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.SetBearerToken(await context.GetTokenAsync("access_token"));

            var response =  await httpClient.GetAsync($"{_serviceUrlsSettings.WebApiEndpoint}/identity");
            return await  response.Content.ReadAsStringAsync();
        }
    }
}
