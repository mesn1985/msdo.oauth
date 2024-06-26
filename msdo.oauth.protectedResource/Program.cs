using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Configuration;
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;

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
    configuration.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext()
);

builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    /// Never use this implementation for other purposes than experimenting locally with identity server.
    /// TLS is disable. The implementation is purely for an educational purpose. And not using TLS
    /// will make it easier to incoporate sniffing tools later.
    options.RequireHttpsMetadata = false;
    options.Authority 
        = builder.Configuration.GetValue<string>("Services:AuthorizationServer");
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "protectedResource");
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments("./OpenApiSpecifications/specifications.xml");

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
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


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Logger.LogInformation($"Starting service with configurations from: {configurationFileName}");
app.Run();
