namespace WebApplication1.DTOS.Payment
{
    public class CreatePaymentDTO
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public int? CurrencyId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentType { get; set; }
    }
}