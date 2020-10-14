using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Front
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel((context, serverOptions) =>
                    {
                        serverOptions.Configure(context.Configuration.GetSection("Kestrel"))
                        .Endpoint("HTTPS", listenOptions =>
                        {
                            listenOptions.HttpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        });
                    });
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ConfigureHttpsDefaults(listenOptions =>
                        {
                            listenOptions.ServerCertificate = CertificateLoader.LoadFromStoreCert("Ws-PC-70", "Root", StoreLocation.LocalMachine, false);
                        });
                    });
                });
    }
}
