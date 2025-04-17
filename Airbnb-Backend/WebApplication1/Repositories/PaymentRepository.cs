using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using WebApplication1.Configurations;
using WebApplication1.DTOS.Payment;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPayment
    {
        #region Dependency Injection
        private readonly AirbnbDBContext _context;
        private readonly IMapper _mapper;
        private readonly StripeSettings _stripeSettings;
        private readonly string _secretKey;

        public PaymentRepository(AirbnbDBContext context, IMapper mapper, IOptions<StripeSettings> stripeSettings) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }
        #endregion

        #region Stripe
        public async Task<PaymentIntent> CreateStripePaymentIntentAsync(CreatePaymentDTO dto)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(dto.Amount * 100),
                Currency = "usd",
                PaymentMethod = "pm_card_visa", // Replace with actual payment method ID
                //ConfirmationMethod = "manual",
                //Confirm = true,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never"
                },
                Metadata = new Dictionary<string, string>
                {
                    { "BookingId", dto.BookingId.ToString() },
                  //  { "UserId", dto.UserId.ToString() }
                }
            };
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
            return paymentIntent;
        }
        #endregion

        //public IEnumerable<Payment> GetUserPayments(Guid userId)
        //{
        //    return _context.Payments
        //        .Include(p => p.Booking)
        //        .Include(p => p.Currency)
        //        .Include(p => p.PaymentMethod)
        //        .Where(p => p.UserId == userId)
        //        .ToList();
        //}

        //public bool ProcessPayment(Guid bookingId, Guid userId, int paymentMethodId, decimal amount)
        //{
        //    var booking = _context.Bookings.Find(bookingId);
        //    if (booking == null || booking.GuestId != userId)
        //        return false;

        //    var payment = new Payment
        //    {
        //        UserId = userId,
        //        BookingId = bookingId,
        //        Amount = amount,
        //        PaymentMethodId = paymentMethodId,
        //        Status = "Completed",
        //        PaymentDate = DateTime.Now,
        //       // CurrencyId = booking.CurrencyId ?? 1
        //    };

        //    _context.Payments.Add(payment);

        //    // Update booking status
        //    //booking.Status = "Confirmed";
        //    booking.UpdatedAt = DateTime.Now;

        //    _context.SaveChanges();
        //    return true;
        //}

        //public IEnumerable<PaymentMethod> GetUserPaymentMethods(Guid userId)
        //{
        //    // In a real app, you would filter by user
        //    return _context.PaymentMethods.ToList();
        //}

        //public bool AddPaymentMethod(Guid userId, PaymentMethod method)
        //{
        //    _context.PaymentMethods.Add(method);
        //    _context.SaveChanges();
        //    return true;
        //}

        //public Payment GetById(Guid id)
        //{
        //    return _context.Payments
        //        .Include(p => p.Booking)
        //        .Include(p => p.PaymentMethod)
        //        .Include(p => p.Currency)
        //        .FirstOrDefault(p => p.Id == id);
        //}
    }
}
