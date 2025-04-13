using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class BookingRepository: GenericRepository<Booking>, IBookingRepository
    {
        private readonly AirbnbDBContext _context;
        private readonly IMapper _mapper;
       public BookingRepository(AirbnbDBContext context , IMapper mapper)  :base(context ,mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<Booking> GetUserBookings(Guid userId)
        {
            return _context.Bookings
                .Include(b => b.Listing)
                .Include(b => b.Currency)
                .Where(b => b.GuestId == userId)
                .ToList();
        }

        public Booking GetBookingDetails(Guid id)
        {
            return _context.Bookings
                .Include(b => b.Listing)
                .Include(b => b.Guest)
                .Include(b => b.Currency)
                .FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Booking> GetListingBookings(Guid listingId)
        {
            return _context.Bookings
                .Include(b => b.Guest)
                .Where(b => b.ListingId == listingId)
                .ToList();
        }

        public bool UpdateBookingStatus(Guid id, string status)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
                return false;

            booking.Status = status;
            booking.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public bool IsListingAvailable(Guid listingId, DateTime checkIn, DateTime checkOut)
        {
            return !_context.Bookings.Any(b =>
                b.ListingId == listingId &&
                b.Status != "Cancelled" &&
                ((checkIn >= b.CheckInDate && checkIn < b.CheckOutDate) ||
                 (checkOut > b.CheckInDate && checkOut <= b.CheckOutDate) ||
                 (checkIn <= b.CheckInDate && checkOut >= b.CheckOutDate)));
        }
    }
}
