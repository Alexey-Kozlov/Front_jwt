using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Front.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Front.Repositories;
using Front.Services;
using IdentityModel;
using System.Threading.Tasks;

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
            services.AddAuthentication()
                .AddOpenIdConnect(options =>
                {
                    options.Events.OnAuthorizationCodeReceived = context =>
                    {
                        context.TokenEndpointRequest.ClientAssertionType = OidcConstants.ClientAssertionTypes.JwtBearer;
                        context.TokenEndpointRequest.ClientAssertion = TokenGenerator.CreateClientAuthJwt();
                        return Task.CompletedTask;
                    };
                });
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
