using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using UserService.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(client => new HttpClient
{
    BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
});


// dotnet add package OpenTelemetry.Instrumentation.AspNetCore --prelease
// dotnet add package OpenTelemetry.Extensions.Hosting --prelease
//  dotnet add package OpenTelemetry.Exporter.Console --prelease
// dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol --prelease
// dotnet add package OpenTelemetry.Instrumentation.Http --prelease
// dotnet add package OpenTelemetry.Instrumentation.StackExchangeRedis --prelease

string serviceName = "UsersApi";
string serviceVersion = "1.0";

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("redis")));


builder.Services.AddOpenTelemetryTracing(builder =>
{
    builder
        .AddConsoleExporter()
        .AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"))        
        .AddSource(serviceName)
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRedisInstrumentation();
});


// dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("redis");
    options.InstanceName = "SampleInstance";
});

builder.Services.AddHealthChecks();

var app = builder.Build();


app.UseHttpsRedirection();


var activitySource = new ActivitySource(serviceName, serviceVersion);


app.MapGet("/hello", () =>
{
    using var activity = activitySource.StartActivity();
    activity.SetTag("foo", 1);
    activity.SetTag("bar", "Hello World");
    activity.SetTag("boo", new int[] { 1, 2, 3});

    return "Hello World!";
});


app.MapGet("users", async (HttpClient client, ILogger<Program> logger) =>
{
    logger.LogInformation("Users retrieved from API");

    var users = await client.GetFromJsonAsync<List<User>>("/users");

    return users;
});


// dotnet add package StackExchange.Redis

app.MapGet("users/cached", async (HttpClient client, 
    IConnectionMultiplexer connectionMultiplexer, 
    ILogger<Program> logger) =>
{

    var db = connectionMultiplexer.GetDatabase();

    var data = await db.StringGetAsync("users");

    if (!data.IsNull)
    {
        logger.LogInformation("Users retrieved from Redis");

        var users = JsonSerializer.Deserialize<List<User>>(data);

        return users;
    }

    else
    {

        logger.LogInformation("Users retrieved from external API");

        var users = await client.GetFromJsonAsync<List<User>>("/users");

        await db.StringSetAsync("users", JsonSerializer.Serialize(users), TimeSpan.FromSeconds(120));

        return users;
    }
});


// https://learn.microsoft.com/pl-pl/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0#distributed-redis-cache

app.MapGet("/distributed-cached-users", async (HttpClient client, IDistributedCache cache, ILogger<Program> logger) =>
{
    var data = await cache.GetAsync("users");

    if (data!=null)
    {
        logger.LogInformation("Users retrieved from Redis");

        var users = JsonSerializer.Deserialize<List<User>>(Encoding.UTF8.GetString(data));

        return users;
    }

    else
    {

        logger.LogInformation("Users retrieved from external API");

        var users = await client.GetFromJsonAsync<List<User>>("/users");

        var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(120));

        await cache.SetStringAsync("users", JsonSerializer.Serialize(users), options);

        return users;
    }
});

app.MapHealthChecks("/health");

app.Run();

