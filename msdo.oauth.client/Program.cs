using msdo.oauth.client.Interfaces;
using msdo.oauth.client.Services;
using Serilog;
using System.Configuration;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

var builder = WebApplication.CreateBuilder(args);

// Load configuration file
string configurationDirectory = $"./ConfigurationFiles/";
string configurationFileName 
    = builder.Configuration.GetValue<string>("ConfigurationFile");

if (string.IsNullOrEmpty(configurationFileName))
{
    throw new ConfigurationException(
        $"Configuration file provide by commandline argument {builder.Configuration.GetValue<string>("ConfigurationFile")} not found"
        );
}

builder.Configuration.AddJsonFile(configurationDirectory + configurationFileName);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext()
    );


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<IProtectedResourceService, ProtectedResourceService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Services:ProtectedResourceServer"));
});
builder.Services.AddHttpClient<IAuthorizationService, IdentityServerAuthorizationService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Services:AuthorizationServerDiscoveryEndpoint"));
    }
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation($"Starting service with configurations from: {configurationFileName}");
app.Run();
