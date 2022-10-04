using Microsoft.Extensions.Primitives;
using OCRService.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
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
app.UseAuth();


app.Use(async (context, next) =>
{
    if (context.Request.Path == "/api/ocr")
    {
        await context.Response.WriteAsync("OCR");
    }
    else
    {
        await next();
    }
});

app.Run(async context => await context.Response.WriteAsync("Hello World!"));


app.Run();
