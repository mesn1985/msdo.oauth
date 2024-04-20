using msdo.oauth.client.Interfaces;
using msdo.oauth.client.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration file

builder.Configuration.AddJsonFile("./ConfigurationFiles/Default.json");

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

app.Run();
