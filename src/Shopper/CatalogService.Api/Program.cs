using CatalogService.Api.AuthorizationHandlers;
using CatalogService.Api.AuthorizationRequirements;
using CatalogService.Api.HealthChecks;
using CatalogService.Api.Middlewares;
using CatalogService.Domain;
using CatalogService.Infrastructure;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Formatting.Compact;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Configuration.AddJsonFile("appsettings.staging.json", optional: true);
builder.Configuration.AddXmlFile("appsettings.xml", optional: true);
builder.Configuration.AddCommandLine(args);
builder.Configuration.AddUserSecrets<Program>();

var awsKey = builder.Configuration["AWS:SecretKey"];


// string npbApiUrl =  builder.Configuration["NBPApiUrl"];
// string npbApiUrl = builder.Configuration["NBPApi:Url"];
// string npbApiTable = builder.Configuration["NBPApi:Table"];
// string npbApiFormat = builder.Configuration["NBPApi:Format"];

// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

string secretKey = "your-256-bit-secret";
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = "http://myauthapi.com",
            ValidateAudience = true,
            ValidAudience = "http://myshopper.com"
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Adult", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireMinimumAge(18);
    });
});


builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddScoped<IAuthorizationHandler, TheSameOwnerHandler>();

// dotnet add package Serilog.AspNetCore
builder.Host.UseSerilog((context, logger) =>
{
    logger.WriteTo.Console();
    logger.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
    logger.WriteTo.File(new CompactJsonFormatter(), "logs/log.json");

    // dotnet add package Serilog.Sinks.Seq
    logger.WriteTo.Seq("http://localhost:5341");

    logger.Enrich.WithProperty("Name", "CatalogService");
    logger.Enrich.WithMemoryUsage();
});

builder.Services.Configure<NbpApiOptions>(builder.Configuration.GetSection("NBPApi"));


// Add services to the container.

// dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter(camelCaseText: true));

    })
    .AddXmlSerializerFormatters();
    ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddSingleton<IMessageService, FakeMessageService>();

builder.Services.AddResponseCompression(options =>
{    
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

// dotnet add package MediatR.Extensions.Autofac.DependencyInjection
builder.Services.AddMediatR(typeof(Program));

// Health Check
builder.Services.AddHealthChecks()
    .AddCheck("Ping", () => HealthCheckResult.Healthy() )
    .AddCheck<NbpApiHealthCheck>("nbpapi")
    .AddCheck("Random", () =>
    {
        if (DateTime.Now.Minute % 2 == 0)
        {
            return HealthCheckResult.Healthy();
        }
        else
        {
            return HealthCheckResult.Unhealthy(description: "Test", new ApplicationException("My Exception"));
        }
    });


builder.Services.AddTransient<AddVersionMiddleware>();

//if (builder.Environment.IsDevelopment())
//{
//    
//    builder.Services.AddSingleton<IProductRepository, DbProductRepository>();
//}

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();


app.MapControllers();

//app.Use(async (context, next) =>
//{
//    context.Response.OnStarting(() =>
//    {
//        context.Response.Headers.Add("X-Version", "1.0");

//        return Task.CompletedTask;
//    });

//    await next();
//});


// W?asny middleware do wersjonowania
app.UseVersion();

// Biblioteka - patrz 
// https://github.com/dotnet/aspnet-api-versioning/wiki


app.MapGet("api/ping", context => context.Response.WriteAsync("Pong"));

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});




app.Run();
