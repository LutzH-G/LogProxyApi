#pragma warning disable CS1591 // Fehledes XML-Kommentar f�r �ffentlich sichtbaren Typ oder Element
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogProxyApi
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
                    webBuilder.ConfigureLogging((ctx, logging) =>
                    {
                        logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                    });
                });
    }
}