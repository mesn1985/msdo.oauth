using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace msdo.oauth.identityServer
{
    public class Program
    {
        public static int Main(string[] args)
        {

            try
            {
               CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args).Build();

          return  Host.CreateDefaultBuilder(args)
              
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.SetBasePath(Directory.GetCurrentDirectory() + "\\ConfigurationFiles\\");
                    hostConfig.AddCommandLine(args);
                    hostConfig.AddJsonFile(config.GetValue<string>("ConfigurationFile"), optional: false);
                })
                .UseSerilog((context, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext()
                )
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}