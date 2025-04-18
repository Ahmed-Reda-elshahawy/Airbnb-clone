using Stripe;
using WebApplication1.DTOS.Payment;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IPayment : IRepository<Payment>
    {
        Task<PaymentResponseDTO> CreatePaymentAsync((PaymentIntent intent, Charge charge, ConfirmPaymentDTO dto) source);
        Task HandlePostPaymentSuccess(Guid bookingId);
        //IEnumerable<Payment> GetUserPayments(Guid userId);
        //bool ProcessPayment(Guid bookingId, Guid userId, int paymentMethodId, decimal amount);
        //IEnumerable<PaymentMethod> GetUserPaymentMethods(Guid userId);
        //bool AddPaymentMethod(Guid userId, PaymentMethod method);
        //Payment GetById(Guid id);
    }
}
