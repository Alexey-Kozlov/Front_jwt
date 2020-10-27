using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Front_jwt.Services
{
    public class MyTokenHandler : DelegatingHandler
    {
        //private readonly IIdentityServerClient _identityServerClient;
        /*Здесь используем не прямое обычное обращение к сервису IIdentityServerClient, а через посредника - IHttpContextAccessor
         * Причина - нам необходимо при каждом запросе к identityServer отправлять запрос с новым параметром jti - это GUID,
         * но базовый класс DelegatingHandler - он не поддерживает Transient, только Scoped из соображений производительности
         * поэтому приходится принудительно при каждом запросе получать экземпляр IdentityServerClient через IHttpContextAccessor.
         * А IdentityServerClient в свою очередь вызывает ClientCredentialsTokenRequest из startup, где генерится новый запрос на код доступа.
         */
        private readonly IHttpContextAccessor _accessor;

        public MyTokenHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var service = _accessor.HttpContext.RequestServices.GetRequiredService<IIdentityServerClient>();
            var accessToken = await service.RequestClientCredentialsTokenAsync();
            request.SetBearerToken(accessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
