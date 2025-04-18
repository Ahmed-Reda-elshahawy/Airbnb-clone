using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using WebApplication1.Configurations;
using WebApplication1.DTOS.AvailabilityCalendar;
using WebApplication1.DTOS.Payment;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Enums;

namespace WebApplication1.Repositories.Payment
{
    public class PaymentRepository : GenericRepository<Models.Payment>, IPayment
    {
        #region Dependency Injection
        private readonly AirbnbDBContext _context;
        private readonly IMapper _mapper;
        private readonly IBooking _bookingRepository;
        private readonly IAvailabilityCalendar _availabilityCalendarRepository;


        public PaymentRepository(AirbnbDBContext context, IMapper mapper, IBooking bookingRepository, IAvailabilityCalendar availabilityCalendarRepository) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _availabilityCalendarRepository = availabilityCalendarRepository;
        }
        #endregion

        #region Create Payment
        public async Task<PaymentResponseDTO> CreatePaymentAsync((PaymentIntent intent, Charge charge, ConfirmPaymentDTO dto) source)
        {
            var createPaymentDto = _mapper.Map<CreatePaymentDTO>(source);

            createPaymentDto.PaymentMethodId = int.Parse(source.intent.Metadata["paymentMethodId"]);
            createPaymentDto.PaymentType = Enum.Parse<PaymentType>(source.intent.Metadata["paymentType"]);
            createPaymentDto.BookingId = Guid.Parse(source.intent.Metadata["bookingId"]);
            createPaymentDto.CurrencyId = int.Parse(source.intent.Metadata["currency"]);
            createPaymentDto.UserId = GetCurrentUserId();

            var payment = _mapper.Map<Models.Payment>(createPaymentDto);
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return _mapper.Map<PaymentResponseDTO>(payment);
        }
        #endregion

        #region Handle Post Payment Success
        public async Task HandlePostPaymentSuccess(Guid bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId) ?? throw new Exception("Booking not found");
            booking.Status = BookingStatus.Confirmed;

            await _availabilityCalendarRepository.MarkDatesUnavailable(booking.ListingId, booking.CheckInDate, booking.CheckOutDate);

            await _context.SaveChangesAsync();
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
