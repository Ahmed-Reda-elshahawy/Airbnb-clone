using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly AirbnbDBContext _context;
        private readonly IMapper _mapper;


        public PaymentRepository(AirbnbDBContext context, IMapper mapper) : base(context,mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<Payment> GetUserPayments(Guid userId)
        {
            return _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.Currency)
                .Include(p => p.PaymentMethod)
                .Where(p => p.UserId == userId)
                .ToList();
        }

        public bool ProcessPayment(Guid bookingId, Guid userId, int paymentMethodId, decimal amount)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null || booking.GuestId != userId)
                return false;

            var payment = new Payment
            {
                UserId = userId,
                BookingId = bookingId,
                Amount = amount,
                PaymentMethodId = paymentMethodId,
                Status = "Completed",
                PaymentDate = DateTime.Now,
                CurrencyId = booking.CurrencyId ?? 1
            };

            _context.Payments.Add(payment);

            // Update booking status
            booking.Status = "Confirmed";
            booking.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return true;
        }

        public IEnumerable<PaymentMethod> GetUserPaymentMethods(Guid userId)
        {
            // In a real app, you would filter by user
            return _context.PaymentMethods.ToList();
        }

        public bool AddPaymentMethod(Guid userId, PaymentMethod method)
        {
            _context.PaymentMethods.Add(method);
            _context.SaveChanges();
            return true;
        }

        public Payment GetById(Guid id)
        {
            return _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.PaymentMethod)
                .Include(p => p.Currency)
                .FirstOrDefault(p => p.Id == id);
        }
    }
}
