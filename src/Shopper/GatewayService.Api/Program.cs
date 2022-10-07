
// Alternatywne rozwi¹zanie: Ocelot
// https://code-maze.com/aspnetcore-api-gateway-with-ocelot/


using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("yarp.json", optional: false, reloadOnChange: true);

// dotnet add package Yarp.ReverseProxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builder =>
    {
        builder.AddRequestTransform(async context =>
        {
            context.ProxyRequest.Headers.Add("X-Custom-Header", "CustomValue");
        });
    });

var app = builder.Build();


app.MapGet("/", () => "Hello Gateway API!");

app.MapReverseProxy();

app.Run();
