using msdo.oauth.client.Interfaces;
using msdo.oauth.client.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Log configuration

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

builder.Configuration.AddJsonFile(configurationDirectory+configurationFileName);
//builder.Configuration.AddJsonFile("./ConfigurationFiles/Local.json");

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IAuthorizationService, IdentityServerAuthorizationService>();
builder.Services.AddScoped<IProtectedResourceService, ProtectedResourceHttpService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
