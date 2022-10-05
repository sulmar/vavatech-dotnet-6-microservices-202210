
using Bogus;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrackingService.Domain;

Console.BackgroundColor = ConsoleColor.DarkGreen;
Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Hello, Signal-R Sender!");

const string url = "https://localhost:5041/signalr/messages";

// dotnet add package Microsoft.AspNetCore.SignalR.Client

HubConnection connection = new HubConnectionBuilder()
    .WithUrl(url)
    .AddMessagePackProtocol()
    .WithAutomaticReconnect()
    .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Trace))
    .Build();

connection.Reconnecting += Connection_Reconnecting;
connection.Reconnected += Connection_Reconnected;

Task Connection_Reconnecting(Exception? arg)
{
    Console.WriteLine($"Reconnecting... {arg.Message}");
    return Task.CompletedTask;
}

Task Connection_Reconnected(string? arg)
{
    Console.WriteLine($"Reconnected.");

    return Task.CompletedTask;
}

Console.WriteLine($"Connecting... {url}");
await connection.StartAsync();
Console.WriteLine($"Connected. {connection.ConnectionId}");


Faker<Message> faker = new Faker<Message>()
    .RuleFor(p => p.Title, f => f.Lorem.Slug())
    .RuleFor(p => p.Content, f => f.Lorem.Sentence());

var messages = faker.GenerateForever();

foreach(var message in messages)
{    
    Console.WriteLine($"Sending... {message.Title} {message.Content}");

    await connection.SendAsync("SendMessage", message);
    Console.WriteLine("Sent.");

    await Task.Delay(TimeSpan.FromMilliseconds(100));
}

Console.ResetColor();

Console.WriteLine("Press any key to exit.");
Console.ReadKey();

