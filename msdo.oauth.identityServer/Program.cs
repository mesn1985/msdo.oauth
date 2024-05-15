using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

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
           

            string configurationDirectory = $"./ConfigurationFiles/";
            string configurationFileName
                = config.GetValue<string>("ConfigurationFile");

            return  Host.CreateDefaultBuilder(args)
              
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.AddCommandLine(args);
                    hostConfig.AddJsonFile(configurationDirectory + configurationFileName, optional: false);
                })
                .UseSerilog((context, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext()
                )
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}