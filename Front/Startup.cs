using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityModel.Client;
using Front_jwt.Models;
using Front_jwt.Services;
using IdentityModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace Front
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            var certSettings = Configuration.GetSection("Certificates").Get<CertificatesSettings>();
            var urlSettings = Configuration.GetSection("ServiceUrls").Get<ServiceUrlsSettings>();
            services.Configure<CertificatesSettings>(Configuration.GetSection("Certificates"));
            services.Configure<ServiceUrlsSettings>(Configuration.GetSection("ServiceUrls"));

            //регистрируется как сервис в контроллере и вызывается всякий раз при обращении к контроллеру
            //кроме варианта с работой через сессии - в сессиях сохраняются значения токенов
            services.AddScoped(typeof(ClientCredentialsTokenRequest), request =>
            {
                return new ClientCredentialsTokenRequest
                {
                    Address = $"{urlSettings.AuthorityApiEndpoint}/connect/token",
                    GrantType = OidcConstants.GrantTypes.ClientCredentials,                    
                    Scope = "api",                                         
                    ClientAssertion = new ClientAssertion
                    {                        
                        Type = OidcConstants.ClientAssertionTypes.JwtBearer,                        
                        Value = TokenGenerator.CreateClientAuthJwt(certSettings.Path, certSettings.Password,
                          urlSettings.AuthorityApiEndpoint)
                    }
                };
            });

            //сервис с дополнительным добавлением значений токенов в заголовки
            services.AddHttpClient<IAddHeaderClient, AddHeaderClient>(client =>
            {
                client.BaseAddress = new Uri(urlSettings.AuthorityApiEndpoint);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<MyTokenHandler>();

            //для добавления значений токенов в заголовки запросов
            services.AddTransient<MyTokenHandler>();
            //необходим для MyTokenHandler
            services.AddHttpContextAccessor();
            //сервис для обработки каждого запроса 
            services.AddHttpClient<IIdentityServerClient, IdentityServerClient>(client =>
            {
                client.BaseAddress = new Uri(urlSettings.AuthorityApiEndpoint);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            //время жизни сессиий
            services.AddSession();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }                     
            
            app.UseRouting();
            app.UseSession();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
