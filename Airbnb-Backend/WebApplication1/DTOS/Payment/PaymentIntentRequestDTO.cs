using WebApplication1.Models.Enums;

namespace WebApplication1.DTOS.Payment
{
    public class PaymentIntentRequestDTO
    {
        public int CurrencyId { get; set; }
        public PaymentType PaymentType { get; set; }
        public int PaymentMethodId { get; set; }
    }
}