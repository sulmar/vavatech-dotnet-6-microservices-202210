using Bogus;
using Grpc.Core;

namespace WarehouseService.Api.Services
{
    public class MyWarehouseService : WarehouseService.WarehouseServiceBase
    {
        public override Task<GetProductStateResponse> GetProductState(GetProductStateRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.ProductId} {request.Color}");

            var response = new GetProductStateResponse { IsAvailable = true, Quantity = 10 };

            return Task.FromResult(response);
        }

        public override async Task SubscribeProductState(GetProductStateRequest request, IServerStreamWriter<GetProductStateResponse> responseStream, ServerCallContext context)
        {
            // dotnet add package Bogus
            var responses = new Faker<GetProductStateResponse>()
                .RuleFor(p => p.Quantity, f => f.Random.Int(0, 100))
                .RuleFor(p => p.IsAvailable, (f, response) => response.Quantity > 0)
                .GenerateForever();

            foreach (var response in responses)
            {
                await responseStream.WriteAsync(response);
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
