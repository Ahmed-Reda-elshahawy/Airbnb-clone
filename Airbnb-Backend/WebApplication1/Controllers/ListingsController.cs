using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.DTOS.Amenity;
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
        public ListingsController(IRepository<Listing> irepo, IMapper mapper, ListingsRepository listingsRepository)
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
            var listings = await _listingsRepository.GetListingsWithDetails(queryParams);
            var listingDTOs = _mapper.Map<List<GetListingDTO>>(listings);
            return Ok(listingDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetListingDTO>> GetListingById(Guid id)
        {
            try
            {
                var listing = await _listingsRepository.GetListingWithDetailsbyId(id);
                if (listing == null)
                {
                    return NotFound("Listing not found.");
                }

                var listingDTOs = _mapper.Map<GetListingDTO>(listing);
                return Ok(listingDTOs);
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
                var listings = await _listingsRepository.GetListingsWithDetails(new Dictionary<string, string> { { "HostId", hostId.ToString() } });

                if (listings == null || !listings.Any())
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

        #region Create Methods
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
                await _irepo.CreateAsync(newListing);
                return CreatedAtAction(nameof(CreateListing), new { id = newListing.Id }, newListing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("{id}/amenities")]
        public async Task<ActionResult<List<ListingAmenity>>> AddAmenitiesToListing(Guid id, [FromBody] List<Guid> amenityIds)
        {
            try
            {
                var updatedListingAmenities = await _listingsRepository.AddAmenitiesToListing(id, amenityIds);
                if (updatedListingAmenities == null || updatedListingAmenities.Count == 0)
                {
                    return NotFound("Listing not found or amenities not found.");
                }
                return Ok(updatedListingAmenities);
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
                var updatedListing = await _irepo.UpdateAsync<Listing, UpdateListingDTO>(id, dto);
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
                await _irepo.DeleteAsync<Listing>(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{listingId}/amenities/{amenityId}")]
        public async Task<ActionResult> DeleteAmenityFromListing(Guid listingId, Guid amenityId)
        {
            try
            {
                var result = await _listingsRepository.RemoveAmenityFromListing(listingId, amenityId);

                if (!result)
                {
                    return NotFound("Listing or Amenity not found.");
                }

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