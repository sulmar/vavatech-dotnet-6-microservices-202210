using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// dotnet add package AspNetCore.HealthChecks.UI 
// dotnet add package AspNetCore.HealthChecks.UI.InMemory.Storage
builder.Services
    .AddHealthChecksUI()
    .AddInMemoryStorage();


var app = builder.Build();


app.MapHealthChecksUI(); // /healthchecks-ui

app.MapGet("/", () => "Hello World!");

app.Run();
