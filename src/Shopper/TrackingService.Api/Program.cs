using TrackingService.Api.Hubs;


// dotnet add package Microsoft.AspNetCore.SignalR.Protocols.MessagePack
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR()
    .AddMessagePackProtocol();

var app = builder.Build();

app.MapGet("/", () => "Use signal-r on signalr/messages");

app.MapHub<StrongTypedMessagesHub>("signalr/messages");

app.Run();
