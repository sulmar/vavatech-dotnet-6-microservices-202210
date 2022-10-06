using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using UserService.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(client => new HttpClient
{
    BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
});


builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("redis")));


// dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("redis");
    options.InstanceName = "SampleInstance";
});

var app = builder.Build();


app.UseHttpsRedirection();


app.MapGet("/users", async (HttpClient client, ILogger<Program> logger) =>
{
    logger.LogInformation("Users retrieved from API");

    var users = await client.GetFromJsonAsync<List<User>>("/users");

    return users;
});


// dotnet add package StackExchange.Redis

app.MapGet("/cached-users", async (HttpClient client, IConnectionMultiplexer connectionMultiplexer, ILogger<Program> logger) =>
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



app.Run();

