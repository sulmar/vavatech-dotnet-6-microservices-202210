using Core.Domain;

namespace PaymentService.Domain
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
    }

    public enum PaymentType
    {
        BankTransfer,
        CreditCard,
        Blik
    }
}