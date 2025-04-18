using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOS.AvailabilityCalendar;
using WebApplication1.DTOS.Listing;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityCalendarController : ControllerBase
    {
        #region Dependency Injection
        private readonly IRepository<AvailabilityCalendar> _irepo;
        private readonly IMapper _mapper;
        private readonly IAvailabilityCalendar _availabilityCalendarRepository;

        public AvailabilityCalendarController(IRepository<AvailabilityCalendar> irepo, IMapper mapper, IAvailabilityCalendar availabilityCalendarRepository)
        {
            _irepo = irepo;
            _mapper = mapper;
            _availabilityCalendarRepository = availabilityCalendarRepository;
        }
        #endregion

        #region Set Availability + Initialize Methods

        #region Initialize availability
        [HttpPost("listings/{listingId}/init")]
        public async Task<IActionResult> InitializeAvailability(Guid listingId, [FromBody] InitAvailabilityCalendarDTO dto)
        {
            try
            {
                var count = await _availabilityCalendarRepository.InitializeAvailabilityAsync(listingId, dto);
                if (count == 0)
                {
                    return NotFound("No availability days to initialize.");
                }
                return Ok(new { message = $"{count} days initialized." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Set availability with Start and End date

        [HttpPost("listings/{listingId}")]
        public async Task<IActionResult> SetAvailability(Guid listingId, SetAvailabilityCalendarDTO dto)
        {
            if (dto.StartDate == default)
            {
                return BadRequest("StartDate is required.");
            }

            if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
            {
                return BadRequest("EndDate cannot be earlier than StartDate.");
            }

            try
            {
                var addedCount = await _availabilityCalendarRepository.SetAvailabilityAsync(listingId, dto);
                return Ok(new { message = $"{addedCount} availability entries set." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        #endregion

        #endregion

        #region Get Methods
        [HttpGet("listings/{listingId}")]
        public async Task<ActionResult<IEnumerable<GetAvailabilityCalendarDTO>>> GetAvailabilityByListingId(Guid listingId)
        {
            try
            {
                var availability = await _availabilityCalendarRepository.GetAllAsync(new Dictionary<string, string> { { "ListingId", listingId.ToString() } });
                if (availability == null || !availability.Any())
                {
                    return NotFound("No availability found for the specified listing.");
                }
                var availabilityDTOs = _mapper.Map<List<GetAvailabilityCalendarDTO>>(availability);
                return Ok(availabilityDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("listings/{listingId}/date/{date}")]
        public async Task<ActionResult<GetAvailabilityCalendarDTO>> GetAvailabilityByListingIdAndDate(Guid listingId, DateTime date)
        {
            try
            {
                var availability = await _availabilityCalendarRepository.GetAllAsync(new Dictionary<string, string>
                {
                    { "ListingId", listingId.ToString() },
                    { "Date", date.ToString() }
                });

                if (availability == null || !availability.Any())
                {
                    return NotFound("No availability found for the specified listing and date.");
                }
                var availabilityDTO = _mapper.Map<IEnumerable<GetAvailabilityCalendarDTO>>(availability);
                return Ok(availabilityDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("listings/{listingId}/available")]
        public async Task<ActionResult<IEnumerable<GetAvailabilityCalendarDTO>>> GetAvailableListings(Guid listingId, [FromQuery] DateTime startDate, [FromQuery] DateTime? endDate = null)
        {
            if (endDate < startDate)
            {
                return BadRequest("EndDate cannot be earlier than StartDate.");
            }
            try
            {
                if (endDate == null)
                {
                   var availableListings = await _availabilityCalendarRepository.GetAvailableListingsAsync(listingId, startDate, startDate);
                    if (availableListings == null || !availableListings.Any())
                    {
                        return NotFound("No available listings found for the given date range.");
                    }
                    var availabilityDTOs = _mapper.Map<List<GetAvailabilityCalendarDTO>>(availableListings);
                    return Ok(availableListings);
                }
                else
                {
                   var availableListings = await _availabilityCalendarRepository.GetAvailableListingsAsync(listingId, startDate, endDate.Value);
                    if (availableListings == null || !availableListings.Any())
                    {
                        return NotFound("No available listings found for the given date range.");
                    }
                    var availabilityDTOs = _mapper.Map<List<GetAvailabilityCalendarDTO>>(availableListings);
                    return Ok(availableListings);

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching available listings: {ex.Message}");
            }
        }
        #endregion

        #region Update Methods
        [HttpPut("listings/{listingId}/date/{date}")]
        public async Task<ActionResult<AvailabilityCalendar>> UpdateAvailability(Guid listingId, DateTime date, [FromBody] UpdateAvailabilityCalendarDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Calendar data is required.");
            }
            var updated = await _availabilityCalendarRepository.UpdateAvailabilityAsync(listingId, date, dto);
            return updated ? Ok("Updated successfully.") : NotFound("Availability entry not found.");
        }
        [HttpPost("listings/{listingId}/batch")]
        public async Task<IActionResult> BatchUpdate(Guid listingId, [FromBody] List<SetAvailabilityCalendarDTO> updates)
        {
            if (updates.Count == 0) return BadRequest("No entries to update.");

            var count = await _availabilityCalendarRepository.BatchUpdateAvailabilityAsync(listingId, updates);
            return Ok(new { message = $"{count} entries updated." });
        }
        #endregion

    }
}

