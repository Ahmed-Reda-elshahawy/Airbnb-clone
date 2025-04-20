using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using WebApplication1.Configurations;
using WebApplication1.DTOS.Payment;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Enums;

namespace WebApplication1.Repositories.Payment
{
    public class StripeRepository : GenericRepository<Models.Payment>, IStripe
    {
        #region dependency injection
        private readonly StripeClient _stripeClient;
        private readonly AirbnbDBContext _context;
        private readonly IBooking _bookingRepository;
        
        public StripeRepository(IOptions<StripeSettings> stripeSettings, AirbnbDBContext context,IMapper mapper, IBooking bookingRepository) : base(context, mapper)
        {
            _context = context;
            _stripeClient = new StripeClient(stripeSettings.Value.SecretKey);
            _bookingRepository = bookingRepository;
        }
        #endregion

        #region Create Payment Intent
        public async Task<PaymentIntentResponseDTO> CreatePaymentIntentAsync(Guid bookingId,PaymentIntentRequestDTO request)
        {
            var currentUserId = GetCurrentUserId();
            var booking = await _bookingRepository.GetByIDAsync(bookingId) ?? throw new Exception("Booking not found");

            if (booking.GuestId != currentUserId)
                throw new Exception("Unauthorized");
            var user = await _context.Users
                            .Include(u => u.Currency)
                            .FirstOrDefaultAsync(u => u.Id == currentUserId)
                            ?? throw new Exception("User not found");

            if (user.Currency == null || string.IsNullOrWhiteSpace(user.Currency.Code))
                throw new Exception("User does not have a valid currency set");

            var currency = user.Currency.Code.ToLower();

            var method = await _context.PaymentMethods.FindAsync(request.PaymentMethodId) ?? throw new Exception("Invalid payment method");

            decimal amountToCharge = request.PaymentType switch
            {
                PaymentType.AllNow => booking.TotalPrice,
                PaymentType.PartNowPartLater => booking.TotalPrice * 0.5m,
                _ => throw new Exception("Invalid payment type")
            };
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amountToCharge * 100),
                Currency = currency,
                PaymentMethod = method.stripeId,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                    AllowRedirects = "never"
                },
                Metadata = new Dictionary<string, string>
                {
                    { "paymentMethodId", request.PaymentMethodId.ToString() },
                    { "paymentType", request.PaymentType.ToString() },
                    { "currency", user.CurrencyId.ToString() },
                    { "bookingId", bookingId.ToString() }
                }
            };

            var service = new PaymentIntentService(_stripeClient);
            var intent = await service.CreateAsync(options);
            booking.PaymentIntentId = intent.Id;
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return new PaymentIntentResponseDTO
            {
                ClientSecret = intent.ClientSecret,
                Status = intent.Status,
                PaymentIntentId = intent.Id
            };
        }
        #endregion

        #region Cancel Payment Intent
        public async Task CancelPaymentIntentAsync(string paymentIntentId)
        {
            var service = new PaymentIntentService(_stripeClient);

            var cancelOptions = new PaymentIntentCancelOptions();
            var paymentIntent = await service.CancelAsync(paymentIntentId, cancelOptions);

            if (paymentIntent.Status != "canceled")
            {
                throw new Exception("Failed to cancel the payment intent.");
            }
        }
        #endregion

        #region Confirm Stripe Payment Intent
        public async Task<(PaymentIntent intent, Charge charge)> ConfirmPaymentStripeAsync(Guid bookingId, ConfirmPaymentDTO dto)
        {
            var currentUserId = GetCurrentUserId();
            var booking = await _bookingRepository.GetByIDAsync(bookingId)
                ?? throw new Exception("Booking not found");

            if (booking.GuestId != currentUserId)
                throw new Exception("You are not authorized to confirm this payment.");

            var service = new PaymentIntentService(_stripeClient);
            var intent = await service.GetAsync(dto.PaymentIntentId);

            if (intent.Status == "requires_confirmation" || intent.Status == "requires_action")
            {
                if (!int.TryParse(intent.Metadata["paymentMethodId"], out int paymentMethodId))
                {
                    throw new Exception("Invalid payment method ID.");
                }

                var method = await _context.PaymentMethods.FindAsync(paymentMethodId) ?? throw new Exception("Payment method not found.");

                var confirmOptions = new PaymentIntentConfirmOptions
                {
                    PaymentMethod = method.stripeId 
                };
                intent = await service.ConfirmAsync(dto.PaymentIntentId, confirmOptions);
            }

            if (intent.Status != "succeeded")
            {
                throw new Exception("Payment failed or not yet completed.");
            }

            if (string.IsNullOrEmpty(intent.LatestChargeId))
                throw new Exception("No charge was created");

            var chargeService = new ChargeService(_stripeClient);
            var charge = await chargeService.GetAsync(intent.LatestChargeId);

            return (intent, charge);
        }
        #endregion

        #region refund Payment
        public async Task RefundAsync(string transactionId, long amountInCents)
        {
            var refundService = new RefundService(_stripeClient);
            var refundOptions = new RefundCreateOptions
            {
                Charge = transactionId,
                Amount = amountInCents
            };

            var refund = await refundService.CreateAsync(refundOptions);

            if (refund.Status != "succeeded")
                throw new Exception("Stripe refund failed.");
        }
        #endregion
    }
}

