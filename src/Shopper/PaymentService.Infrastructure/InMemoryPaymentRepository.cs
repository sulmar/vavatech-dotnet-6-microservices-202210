using Core.Infrastructure;
using PaymentService.Domain;

namespace PaymentService.Infrastructure
{
    public class InMemoryPaymentRepository : InMemoryEntityRepository<Payment>, IPaymentRepository
    {

        public InMemoryPaymentRepository()
        {
            _entities = new List<Payment>
            {
                new Payment { Id = 1, Amount = 100, PaymentType = PaymentType.Blik },
                new Payment { Id = 2, Amount = 200, PaymentType = PaymentType.BankTransfer },
                new Payment { Id = 3, Amount = 300, PaymentType = PaymentType.CreditCard },
            };
        }
    }
}