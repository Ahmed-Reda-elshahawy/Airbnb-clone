using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingControllerController : ControllerBase 
    {
       
        private readonly IBookingRepository _bookingRepository;
        private readonly ListingsRepository _listingRepository;

        public BookingControllerController(IBookingRepository bookingRepository, ListingsRepository listingRepository )
        {
            _bookingRepository = bookingRepository;
            _listingRepository = listingRepository;
        }

        // POST: api/bookings
        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse("5a4eca16-8865-42fe-9019-7a0167ed0858");
            booking.GuestId = userId;
            booking.Status = "Pending";
            booking.BookingDate = DateTime.Now;

            // Validate listing availability
            var listing = _listingRepository.GetListingWithDetailsbyId(booking.ListingId);
            if (listing == null)
                return NotFound("Listing not found");

            if (!_bookingRepository.IsListingAvailable(booking.ListingId, booking.CheckInDate, booking.CheckOutDate))
                return BadRequest("Listing not available for selected dates");

            _bookingRepository.Create(booking);
           _bookingRepository.Save();

            return CreatedAtAction(nameof(GetBookingDetails), new { id = booking.Id }, booking);
        }

        // GET: api/bookings/me
        [HttpGet("me")]
        public IActionResult GetUserBookings()
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            var bookings = _bookingRepository.GetUserBookings(userId);
            return Ok(bookings);
        }

        // GET: api/bookings/{id}
        [HttpGet("{id}")]
        public IActionResult GetBookingDetails(Guid id)
        {
            var booking = _bookingRepository.GetBookingDetails(id);
            if (booking == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            if (booking.GuestId != userId && booking.Listing.HostId != userId)
                return Forbid();

            return Ok(booking);
        }

        // PUT: api/bookings/{id}/status
        [HttpPut("{id}/status")]
        public IActionResult UpdateBookingStatus(Guid id, [FromBody] string status)
        {
            var booking = _bookingRepository.GetBookingDetails(id);
            if (booking == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            if (booking.Listing.HostId != userId)
                return Forbid();

            if (!_bookingRepository.UpdateBookingStatus(id, status))
                return BadRequest("Failed to update booking status");

            return NoContent();
        }

        // DELETE: api/bookings/{id}
        [HttpDelete("{id}")]
        public IActionResult CancelBooking(Guid id)
        {
            var booking = _bookingRepository.GetBookingDetails(id);
            if (booking == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            if (booking.GuestId != userId)
                return Forbid();

            _bookingRepository.Delete(booking);
            _bookingRepository.Save();

            return NoContent();
        }

        // GET: api/listings/{id}/bookings
        [HttpGet("~/api/listings/{id}/bookings")]
        public IActionResult GetListingBookings(Guid id)
        {
            var listing = _listingRepository.GetListingWithDetailsbyId(id);
            if (listing == null)
                return NotFound("Listing not found");

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);

            //' ADD THIS IF CONTIDTION WHEN YOU ADD LISTING HOST ID TO BOOKING
            //if (listing.HostId != userId)
            // return Forbid();

            var bookings = _bookingRepository.GetListingBookings(id);
            return Ok(bookings);
        }
    }
}