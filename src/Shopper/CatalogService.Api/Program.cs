using CatalogService.Api.HealthChecks;
using CatalogService.Domain;
using CatalogService.Infrastructure;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

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
            return HealthCheckResult.Unhealthy();
        }
    });

// dotnet add package AspNetCore.HealthChecks.UI 
// dotnet add package AspNetCore.HealthChecks.UI.InMemory.Storage
builder.Services
    .AddHealthChecksUI()
    .AddInMemoryStorage();

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

app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();



app.MapGet("api/ping", context => context.Response.WriteAsync("Pong"));

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(); // /healtchecks-ui

app.Run();
