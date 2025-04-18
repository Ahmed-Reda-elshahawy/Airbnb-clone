using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOS.Booking;
using WebApplication1.DTOS.Review;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Enums;

namespace WebApplication1.Repositories
{
    public class BookingRepository: GenericRepository<Booking>, IBooking
    {
        #region Dependency Injection
        private readonly AirbnbDBContext context;
        private readonly IMapper mapper;
        private readonly IAvailabilityCalendar availabilityCalendarRepository;
        public BookingRepository(AirbnbDBContext _context, IMapper _mapper, IAvailabilityCalendar _availabilityCalendarRepository) : base(_context, _mapper)
        {
            context = _context;
            mapper = _mapper;
            availabilityCalendarRepository = _availabilityCalendarRepository;
        }
        #endregion

        #region Create Methods
        public async Task<Booking> CreateBooking(CreateBookingDTO dto)
        {
            try
            {       
                var listing = await context.Listings.FirstOrDefaultAsync(l => l.Id == dto.ListingId) ?? throw new Exception("Listing not found.");
                ValidateBookingDates(dto.CheckInDate, dto.CheckOutDate, listing);

                bool isAvailable = await IsListingAvailable(dto.ListingId, dto.CheckInDate, dto.CheckOutDate);
                if (!isAvailable)
                    throw new Exception("Listing is not available for the selected dates.");
                if(dto.GuestsCount > listing.Capacity)
                    throw new Exception("Guests count exceeds listing capacity.");


                var booking = mapper.Map<Booking>(dto);
                booking.TotalPrice = await CalculateTotalPrice(listing, dto.CheckInDate, dto.CheckOutDate);
                booking.GuestId = GetCurrentUserId();
                booking.Status = BookingStatus.Pending;
             
                await CreateAsync(booking);
                return booking;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating Booking: {ex.Message}");
            }
        }
        #endregion

        #region Helper Methods
        private async Task<bool> IsListingAvailable(Guid listingId, DateTime startDate, DateTime endDate)
        {
            var availableDates = await GetAvailableDates(listingId, startDate, endDate);
            var conflictingBookings = await GetConflictingBookings(listingId, startDate, endDate);

            return availableDates && conflictingBookings;
        }
        private async Task<bool> GetAvailableDates(Guid listingId, DateTime startDate, DateTime endDate)
        {
            var totalDays = (endDate - startDate).Days;
            if (totalDays <= 0) return false;

            var expectedDates = Enumerable.Range(0, totalDays).Select(offset => startDate.AddDays(offset)).ToList();
            var availableDates = await availabilityCalendarRepository.GetAvailableListingsAsync(listingId, startDate, endDate);
            var availableDateList = availableDates.Select(ad => ad.Date.Date).ToList();

            return !expectedDates.Except(availableDateList).Any();
        }
        private async Task<bool> GetConflictingBookings(Guid listingId, DateTime startDate, DateTime endDate)
        {
            var conflictingBookings = await context.Bookings
                .Where(b => b.ListingId == listingId
                            && ((b.CheckInDate < endDate && b.CheckOutDate >= startDate))
                            && (b.Status == BookingStatus.Confirmed))
                .ToListAsync();

            Console.WriteLine($"{conflictingBookings}");
            return conflictingBookings.Count == 0;
        }
        private async Task<decimal> CalculateTotalPrice(Listing listing, DateTime checkIn, DateTime checkOut)
        {
            if (listing == null) throw new Exception("Listing not found.");
            if (listing.PricePerNight <= 0) throw new Exception("Invalid price per night.");

            var days = (checkOut - checkIn).Days;
            if (days <= 0) throw new Exception("Invalid date range.");

            var dates = Enumerable.Range(0, days).Select(offset => checkIn.AddDays(offset)).ToList();
            var calendarEntries = await context.AvailabilityCalendars.Where(ac => ac.ListingId == listing.Id && dates.Contains(ac.Date)).ToListAsync();

            decimal totalPrice = 0;

            foreach (var date in dates)
            {
                var calendar = calendarEntries.FirstOrDefault(ac => ac.Date == date);
                totalPrice += calendar?.SpecialPrice ?? listing.PricePerNight;
            }

            return totalPrice + (listing.ServiceFee ?? 0) + (listing.SecurityDeposit??0);
        }

        private void ValidateBookingDates(DateTime checkInDate, DateTime checkOutDate, Listing listing)
        {
            if (checkInDate < DateTime.UtcNow.Date)
                throw new Exception("Check-in date cannot be in the past.");

            if (checkOutDate < checkInDate)
                throw new Exception("Check-out date cannot be earlier than check-in date.");

            var days = (checkOutDate - checkInDate).Days;
            if (days > listing.MaxNights || days < listing.MinNights)
                throw new Exception($"Maximum stay duration is {listing.MaxNights} days and Minimum is {listing.MinNights}");
        }

        #endregion

        //public IEnumerable<Booking> GetUserBookings(Guid userId)
        //{
        //    return _context.Bookings
        //        .Include(b => b.Listing)
        //        .Include(b => b.Currency)
        //        .Where(b => b.GuestId == userId)
        //        .ToList();
        //}

        //public Booking GetBookingDetails(Guid id)
        //{
        //    return _context.Bookings
        //        .Include(b => b.Listing)
        //        .Include(b => b.Guest)
        //        .Include(b => b.Currency)
        //        .FirstOrDefault(b => b.Id == id);
        //}

        //public IEnumerable<Booking> GetListingBookings(Guid listingId)
        //{
        //    return _context.Bookings
        //        .Include(b => b.Guest)
        //        .Where(b => b.ListingId == listingId)
        //        .ToList();
        //}

        //public bool UpdateBookingStatus(Guid id, string status)
        //{
        //    var booking = _context.Bookings.Find(id);
        //    if (booking == null)
        //        return false;

        //    booking.Status = status;
        //    booking.UpdatedAt = DateTime.Now;
        //    _context.SaveChanges();
        //    return true;
        //}


    }
}
