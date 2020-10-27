using IdentityModel.Client;
using System;
using Microsoft.Extensions.Options;
using Front_jwt.Models;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;

namespace Front_jwt.Services
{
    public class IdentityServerClient : IIdentityServerClient
    {
        private readonly HttpClient _httpClient;
        private readonly ClientCredentialsTokenRequest _tokenRequest;

        public IdentityServerClient(HttpClient httpClient, ClientCredentialsTokenRequest tokenRequest)
        {
            _httpClient = httpClient;
            _tokenRequest = tokenRequest;
        }
        public async Task<string> RequestClientCredentialsTokenAsync()
        {
            //вызывается ClientCredentialsTokenRequest из startup
            var response = await _httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);
            return response.AccessToken;

        }
    }
}
