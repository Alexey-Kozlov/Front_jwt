using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Front_jwt.Models;
using Microsoft.Extensions.Options;

namespace Front_jwt.Services
{
    public class AddHeaderClient : IAddHeaderClient
    {
        //это сервис для получение данных из web api.
        //в нем уже все реализовано, нужно только вызывать нужные методы из контроллеров
        private readonly HttpClient _httpClient;
        private readonly ServiceUrlsSettings _serviceUrlsSettings;

        public AddHeaderClient(HttpClient httpClient, IOptions<ServiceUrlsSettings> serviceUrlsSettings)
        {
            _httpClient = httpClient;
            _serviceUrlsSettings = serviceUrlsSettings.Value;
        }

        public async Task<string> GetdataFromApi()
        {
            var response = await _httpClient.GetAsync($"{_serviceUrlsSettings.WebApiEndpoint}/identity/test");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
