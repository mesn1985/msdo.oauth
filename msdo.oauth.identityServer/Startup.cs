// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using System;
using System.Linq;

namespace msdo.oauth.identityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            //services.AddControllersWithViews();

            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;

            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients);

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            // uncomment if you want to add MVC
            //app.UseStaticFiles();
            //app.UseRouting();
            // Middleware that adds correlation id to request.
            app.Use(async (context, next) =>
            {
                const string _correlationIdHeader = "X-Correlation-Id";
                const string correlationIdLogPropertyname = "CorrelationId";

                context.Request.Headers.TryGetValue(_correlationIdHeader, out StringValues correlationIds);
                var correlationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();

                using (LogContext.PushProperty(correlationIdLogPropertyname, correlationId))
                {
                    context.Items["Correlation-Id"] = correlationId;
                    await next(context);
                }


            });
            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute();
            //});
        }
    }
}
