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
            services.Configure<ServiceUrlsSettings>(Configuration.GetSection("ServiceUrls"));
            services.AddScoped<ITestService, TestService>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie.Name = "identity_server_mvc";
            })
            .AddOpenIdConnect("oidc", options =>
            {
                
                options.SignInScheme = "Cookies";
                options.Authority = Configuration.GetSection("ServiceUrls").GetSection("AuthorityApiEndpoint").Value;
                options.ClientId = "Test";
                options.ClientSecret = "Test";
                options.ResponseType = "code";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.UsePkce = true;
                options.ResponseMode = "query";
                options.Scope.Add("api");
                options.SaveTokens = true;
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }                     

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();

            });
        }
    }
}
