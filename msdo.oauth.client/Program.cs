using msdo.oauth.client.Interfaces;
using msdo.oauth.client.Services;
using msdo.middleware.httpRequest;
using Serilog;
using System.Configuration;

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

//Log configuration
builder.Host.UseSerilog((context, configuration) =>
  configuration.ReadFrom.Configuration(context.Configuration)
      .WriteTo.Console(outputTemplate:
          "[Time:{Timestamp:HH:mm:ss}] [Log level:{Level:u3}] [Correlation id:{CorrelationId}] {NewLine} [Message:{Message}] "
          )
      .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
      .Enrich.FromLogContext()
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

// Middleware that adds correlation id to request, if no correlation id is present, the header name is: X-Correlation-Id
app.UseMiddleware<IncomingCorrelationIdMiddleware>();

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
