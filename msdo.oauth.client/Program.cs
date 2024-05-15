using msdo.oauth.client.Interfaces;
using msdo.oauth.client.Services;
using Serilog;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Load configuration file
string configurationDirectory = $"./ConfigurationFiles/";
string configurationFileName 
    = builder.Configuration.GetValue<string>("ConfigurationFile");

builder.Configuration.AddJsonFile(configurationDirectory + configurationFileName, optional:false);

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
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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
