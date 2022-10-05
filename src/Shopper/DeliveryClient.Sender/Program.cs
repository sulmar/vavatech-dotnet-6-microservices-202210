
using DeliveryService.Contracts;
using Grpc.Net.Client;
using ProtoBuf;
using ProtoBuf.Grpc.Client;

Console.WriteLine("Hello, gRPC Code-First Sender!");

const string url = "https://localhost:5061";

// dotnet add package Grpc.Net.Client
// dotnet add package protobuf-net.Grpc

string proto = Serializer.GetProto<IDeliveryService>();

Console.WriteLine(proto);


using var channel = GrpcChannel.ForAddress(url);

var client = channel.CreateGrpcService<IDeliveryService>();

var request = new ConfirmDeliveryRequest { BoxId = 1, ShippedDate = DateTime.Now, Sign = "John Smith" };

var response = await client.ConfirmDeliveryAsync(request);

Console.WriteLine($"{response.IsConfirmed}");




Console.WriteLine("Press any key to exit.");
Console.ReadKey();