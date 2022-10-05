using DeliveryService.Contracts;
using ProtoBuf.Grpc;

namespace DeliveryService.Api.Services
{
    public class MyDeliveryService : IDeliveryService
    {
        public Task<ConfirmDeliveryResponse> ConfirmDeliveryAsync(ConfirmDeliveryRequest request, CallContext context = default)
        {
            Console.WriteLine($"{request.BoxId} {request.ShippedDate} {request.Sign}");

            var response = new ConfirmDeliveryResponse { IsConfirmed = true };

            return Task.FromResult(response);
        }
    }
}
