using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.DTOS.Listing;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        #region Dependency Injection
        private readonly IRepository<Listing> _irepo;
        private readonly IMapper _mapper;
        private readonly ListingsRepository _listingsRepository;
        public ListingsController(IRepository<Listing> irepo, IMapper mapper,ListingsRepository listingsRepository)
        {
            _irepo = irepo;
            _mapper = mapper;
            _listingsRepository = listingsRepository;
        }
        #endregion

        #region Get Methods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetListingDTO>>> GetAllListings([FromQuery] Dictionary<string, string> queryParams)
        {
            var listings = await _irepo.GetAllAsync(queryParams);  // Call GetAllAsync with query params
            var listingDTOs = _mapper.Map<List<GetListingDTO>>(listings);
            return Ok(listingDTOs);  // Return filtered listings
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetListingDTO>> GetListingById(Guid id)
        {
            try
            {
                var listing = await _irepo.GetByIDAsync(id);
                if (listing == null)
                {
                    return NotFound("Listing not found.");
                }

                var listingDTOs = _mapper.Map<GetListingDTO>(listing);
                return Ok(listing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("host/{hostId}")]
        public async Task<ActionResult<IEnumerable<GetListingDTO>>> GetListingsByHost(Guid hostId)
        {
            try
            {
                var listings = await _listingsRepository.GetListingsByHostAsync(hostId);

                if (listings == null || listings.Count == 0)
                {
                    return NotFound("No listings found for the specified host.");
                }

                var listingDTOs = _mapper.Map<List<GetListingDTO>>(listings);

                return Ok(listingDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        #endregion

        #region Create Method
        [HttpPost]
        public async Task<ActionResult> CreateListing([FromBody] CreateListingDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Listing data is required.");
            }
            try
            {
                var newListing = _mapper.Map<Listing>(dto);
                newListing.CreatedAt = DateTime.UtcNow;  // Set CreatedAt to current UTC time
                newListing.UpdatedAt = DateTime.UtcNow;  // Set UpdatedAt to current UTC time
                newListing.Id = Guid.NewGuid();  // Generate a new unique identifier for the listing
                await _irepo.CreateAsync(newListing);
                return CreatedAtAction(nameof(CreateListing), new { id = newListing.Id }, newListing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        #endregion

        #region Update Methods
        [HttpPut("{id}")]
        //[HttpPatch("{id}")]
        public async Task<ActionResult<Listing>> UpdateListing(Guid id, [FromBody] UpdateListingDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Listing data is required.");
            }
            try
            {
                Console.WriteLine($"Received RoomTypeId: {dto.RoomTypeId}");
                var updatedListing = await _irepo.UpdateAsync<Listing, UpdateListingDTO>(id, dto);

                if (updatedListing == null)
                {
                    return NotFound("Listing not found.");
                }

                updatedListing.UpdatedAt = DateTime.UtcNow;  

                await _irepo.UpdateAsync<Listing, UpdateListingDTO>(id, dto); 

                return Ok(updatedListing);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        #endregion

        #region Delete Method
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteListing(Guid id)
        {
            try
            {
                await _irepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        #endregion

    }
}