
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using TrackingService.Domain;

Console.BackgroundColor = ConsoleColor.DarkBlue;
Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Hello, Signal-R Received!");

const string url = "https://localhost:5041/signalr/messages";

// dotnet add package Microsoft.AspNetCore.SignalR.Client

HubConnection connection = new HubConnectionBuilder()
    .WithUrl(url)
    .AddMessagePackProtocol()
    .Build();

connection.On<Message>("YouHaveGotMessage", 
    message => Console.WriteLine($"{message.Title} {message.Content}"));

Console.WriteLine($"Connecting... {url}");
await connection.StartAsync();
Console.WriteLine($"Connected. {connection.ConnectionId}");

Console.ResetColor();

Console.WriteLine("Press any key to exit.");
Console.ReadKey();

await connection.StopAsync();


