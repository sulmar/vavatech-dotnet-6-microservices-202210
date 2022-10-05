using Grpc.Core;
using Grpc.Net.Client;
using System.ComponentModel.DataAnnotations;
using WarehouseService.Api;

Console.WriteLine("Hello, gRPC Receiver!");


const string url = "https://localhost:5051";

using var channel = GrpcChannel.ForAddress(url);

var client =  new WarehouseService.Api.WarehouseService.WarehouseServiceClient(channel);

var request = new WarehouseService.Api.GetProductStateRequest { ProductId = 1, Color = "Red" };

var response = await client.GetProductStateAsync(request);

Console.WriteLine($"{response.IsAvailable} {response.Quantity}" );

 var subscription = client.SubscribeProductState(request);

var productStates = subscription.ResponseStream.ReadAllAsync();

await foreach (GetProductStateResponse productState in productStates)
{
    Console.WriteLine($"{productState.IsAvailable} {productState.Quantity}");
}

Console.WriteLine("Press any key to exit.");
Console.ReadKey();







