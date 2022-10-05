using Hangfire;
using Microsoft.Extensions.Primitives;
using OCRService.Api.Middlewares;
using OCRService.Domain;
using OCRService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOCRService, MyOCRService>();

// dotnet add package Hangfire.AspNetCore
// dotnet add package Hangfire.InMemory
builder.Services.AddHangfire(options => options.UseInMemoryStorage());

string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HangfireDb;Integrated Security=True";

// dotnet add package Hangfire.SqlServer
builder.Services.AddHangfire(options => options.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

var app = builder.Build();


// Logger Middleware (warstwa poœrednia)
//app.Use(async (context, next) =>
//{
//    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");

//    await next();

//    Console.WriteLine($"{context.Response.StatusCode}");
//});

// Authorization Middleware (warstwa poœrednia)
//app.Use(async (context, next) =>
//{
//    if (context.Request.Headers.TryGetValue("Authorization", out StringValues value) && value == "blabla")
//    {
//        await next();        
//    }
//    else
//    {
//        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//    }
//});

// app.UseMiddleware<LoggerMiddleware>();
// app.UseMiddleware<AuthorizationMiddleware>();

app.UseLogger();
//app.UseAuth();


//app.Use(async (context, next) =>
//{
//    if (context.Request.Path == "/api/ocr")
//    {
//        await context.Response.WriteAsync("OCR");
//    }
//    else
//    {
//        await next();
//    }
//});

app.MapPost("api/ocr/{documentId}", (int documentId, IOCRService oCRService, IBackgroundJobClient jobClient) =>
{
    // BackgroundJob.Enqueue(() => oCRService.Get(documentId));

    jobClient.Enqueue(() => oCRService.Get(documentId));
    
    return Results.Accepted();

});


// app.Run(async context => await context.Response.WriteAsync("Hello World!"));


app.MapHangfireDashboard();

app.Run();
