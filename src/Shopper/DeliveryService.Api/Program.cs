using DeliveryService.Api.Services;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

// dotnet add package protobuf-net.Grpc.AspNetCore
builder.Services.AddCodeFirstGrpc();

// dotnet add package protobuf-net.Grpc.AspNetCore.Reflection
// dotnet add package System.ServiceModel.Primitives
builder.Services.AddCodeFirstGrpcReflection();

var app = builder.Build();

app.MapGrpcService<MyDeliveryService>();
app.MapCodeFirstGrpcReflectionService();

// dotnet tool install -g dotnet-grpc-cli
// dotnet grpc-cli ls https://localhost:5061
// dotnet grpc-cli ls https://localhost:5061 DeliveryService.Contracts.DeliveryService
// dotnet grpc-cli dump https://localhost:5061 DeliveryService.Contracts.DeliveryService

app.MapGet("/", () => "Hello World!");

app.Run();
