using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Domain;

namespace PaymentService.Api.Queries
{
    // GET api/payments
    // dotnet add package Ardalis.ApiEndpoints
    public class GetPaymentsQuery : EndpointBaseAsync.WithoutRequest.WithResult<IEnumerable<Payment>>
    {
        private readonly IPaymentRepository paymentRepository;

        public GetPaymentsQuery(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }


        [HttpGet("api/payments")]
        public override Task<IEnumerable<Payment>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var payments = paymentRepository.Get();

            return Task.FromResult(payments);
        }
    }
}
