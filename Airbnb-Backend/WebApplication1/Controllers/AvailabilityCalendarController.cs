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
        private readonly AvailabilityCalendarRepository _availabilityCalendarRepository;

        public AvailabilityCalendarController(IRepository<AvailabilityCalendar> irepo, IMapper mapper,AvailabilityCalendarRepository availabilityCalendarRepository)
        {
            _irepo = irepo;
            _mapper = mapper;
            _availabilityCalendarRepository = availabilityCalendarRepository;
        }
        #endregion
        #region Post Methods

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
        public async Task<IActionResult> SetAvailability(Guid listingId, [FromBody] SetAvailabilityCalendarDTO dto)
        {
            try
            {
                var result = await _availabilityCalendarRepository.SetAvailabilityRange(listingId, dto);
                return Ok(new { message = $"{result} availability dates set." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error setting availability: " + ex.Message);
            }
        }
        #endregion

        #endregion

        #region Get Methods
        [HttpGet("listing/{listingId}")]
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
        #endregion

    }
}