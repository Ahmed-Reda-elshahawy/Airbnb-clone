using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS.Booking;
using WebApplication1.DTOS.Listing;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        #region Dependency Injection
        private readonly IBooking _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ListingsRepository _listingsRepository;
        public BookingController(IBooking bookingRepository, IMapper mapper, ListingsRepository listingsRepository)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _listingsRepository = listingsRepository;
        }
        #endregion

        #region Post Methods
        [HttpPost]
        public async Task<ActionResult> CreateBooking([FromBody] CreateBookingDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Booking data is required.");
            }
            try
            {
                var newbooking = await _bookingRepository.CreateBooking(dto);
                return CreatedAtAction(nameof(CreateBooking), new { id = newbooking.Id }, newbooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        #endregion

        #region Get Methods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBookingDTO>>> GetAllBookings([FromQuery] Dictionary<string, string> queryParams)
        {
            var bookings = await _bookingRepository.GetAllAsync(queryParams);
            var bookingsDTOs = _mapper.Map<List<GetBookingDTO>>(bookings);
            return Ok(bookingsDTOs);
        }
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<GetBookingDTO>>> GetUserBookings([FromQuery] Dictionary<string, string> queryParams)
        {
            var userId = _bookingRepository.GetCurrentUserId();
            queryParams["GuestId"] = userId.ToString();
            var userBookings = await _bookingRepository.GetAllAsync(queryParams);
            var userBookingsDTOs = _mapper.Map<List<GetBookingDTO>>(userBookings);

            return Ok(userBookingsDTOs);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookingDTO>> GetBookingById(Guid id)
        {
            var booking = await _bookingRepository.GetByIDAsync(id);
            if (booking == null)
                return NotFound();
            var bookingDTO = _mapper.Map<GetBookingDTO>(booking);
            return Ok(bookingDTO);
        }

        [HttpGet("listings/{id}")]
        public async Task<ActionResult<IEnumerable<GetBookingDTO>>> GetListingBookings(Guid id)
        {
            var listing = await _listingsRepository.GetListingWithDetailsbyId(id);
            if (listing == null)
                return NotFound("Listing not found");
            var bookings = await _bookingRepository.GetAllAsync(new Dictionary<string, string> { { "ListingId", id.ToString() } });
            var bookingsDTOs = _mapper.Map<List<GetBookingDTO>>(bookings);
            return Ok(bookingsDTOs);
        }
        #endregion
    }
}

        //// PUT: api/bookings/{id}/status
        //[HttpPut("{id}/status")]
        //public IActionResult UpdateBookingStatus(Guid id, [FromBody] string status)
        //{
        //    var booking = _bookingRepository.GetBookingDetails(id);
        //    if (booking == null)
        //        return NotFound();

        //    var userId = Guid.Parse(User.FindFirst("sub")?.Value);
        //    if (booking.Listing.HostId != userId)
        //        return Forbid();

        //    if (!_bookingRepository.UpdateBookingStatus(id, status))
        //        return BadRequest("Failed to update booking status");

        //    return NoContent();
        //}

        //// DELETE: api/bookings/{id}
        //[HttpDelete("{id}")]
        //public IActionResult CancelBooking(Guid id)
        //{
        //    var booking = _bookingRepository.GetBookingDetails(id);
        //    if (booking == null)
        //        return NotFound();

        //    var userId = Guid.Parse(User.FindFirst("sub")?.Value);
        //    if (booking.GuestId != userId)
        //        return Forbid();

        //    _bookingRepository.Delete(booking);
        //    _bookingRepository.Save();

        //    return NoContent();
        //}