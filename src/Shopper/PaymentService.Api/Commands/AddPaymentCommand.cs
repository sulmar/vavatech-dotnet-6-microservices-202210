using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Api.Queries;
using PaymentService.Domain;

namespace PaymentService.Api.Commands
{
    public class AddPaymentCommand : EndpointBaseAsync.WithRequest<Payment>.WithActionResult<Payment>
    {
        private readonly IPaymentRepository paymentRepository;

        public AddPaymentCommand(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        [HttpPost("api/payments")]
        public async override Task<ActionResult<Payment>> HandleAsync(Payment payment, CancellationToken cancellationToken = default)
        {
            paymentRepository.Add(payment);

            return CreatedAtRoute(nameof(GetPaymentByIdQuery), new { Id = payment.Id }, payment);
        }
    }
}
