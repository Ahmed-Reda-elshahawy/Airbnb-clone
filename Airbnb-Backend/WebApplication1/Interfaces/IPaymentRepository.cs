using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        IEnumerable<Payment> GetUserPayments(Guid userId);
        bool ProcessPayment(Guid bookingId, Guid userId, int paymentMethodId, decimal amount);
        IEnumerable<PaymentMethod> GetUserPaymentMethods(Guid userId);
        bool AddPaymentMethod(Guid userId, PaymentMethod method);
        Payment GetById(Guid id);
    }
}
