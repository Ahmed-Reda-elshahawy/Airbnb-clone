using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.DTOS.Listing;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IRepository<Listing> _irepo;
        public ListingsController(IRepository<Listing> irepo)
        {
            _irepo = irepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> GetAllListings()
        {
            var listings = await _irepo.GetAllAsync();
            if (listings == null || !listings.Any())
            {
                return NoContent();
            }
            return Ok(listings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Listing>> GetListingById(Guid id)
        {
            try
            {
                var listing = await _irepo.GetByIDAsync(id);
                if (listing == null)
                {
                    return NotFound("Listing not found.");
                }

                return Ok(listing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateListing([FromBody] CreateListingDTO createListingDTO)
        {
            if (createListingDTO == null)
            {
                return BadRequest("Listing data is required.");
            }

            try
            {
                var newListing = new Listing
                {
                    Id = Guid.NewGuid(),
                    HostId = createListingDTO.HostId,
                    Title = createListingDTO.Title,
                    Description = createListingDTO.Description,
                    PropertyTypeId = createListingDTO.PropertyTypeId,
                    RoomTypeId = createListingDTO.RoomTypeId,
                    Capacity = createListingDTO.Capacity,
                    Bedrooms = createListingDTO.Bedrooms,
                    Bathrooms = createListingDTO.Bathrooms,
                    PricePerNight = createListingDTO.PricePerNight,
                    ServiceFee = createListingDTO.ServiceFee,
                    AddressLine1 = createListingDTO.AddressLine1,
                    AddressLine2 = createListingDTO.AddressLine2,
                    City = createListingDTO.City,
                    State = createListingDTO.State,
                    Country = createListingDTO.Country,
                    PostalCode = createListingDTO.PostalCode,
                    Latitude = createListingDTO.Latitude,
                    Longitude = createListingDTO.Longitude,
                    InstantBooking = createListingDTO.InstantBooking,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    MinNights = createListingDTO.MinNights,
                    MaxNights = createListingDTO.MaxNights,
                    CancellationPolicyId = createListingDTO.CancellationPolicyId,
                    AverageRating = createListingDTO.AverageRating,
                    ReviewCount = createListingDTO.ReviewCount,
                    IsActive = createListingDTO.IsActive,
                    CurrencyId = createListingDTO.CurrencyId
                };
                await _irepo.CreateAsync(newListing); // Save the new listing to the database

                return CreatedAtAction(nameof(CreateListing), new { id = newListing.Id }, newListing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateListing(Guid id, [FromBody] UpdateListingDTO updateListingDTO)
        {
            if (updateListingDTO is null)
            {
                return BadRequest("Listing data is required.");
            }

            try
            {
                var existingListing = await _irepo.GetByIDAsync(id);
                if (existingListing is null)
                {
                    return NotFound("Listing not found.");
                }

                existingListing.Title = updateListingDTO.Title ?? existingListing.Title;
                existingListing.Description = updateListingDTO.Description ?? existingListing.Description;
                existingListing.UpdatedAt = DateTime.UtcNow;
                existingListing.HostId = updateListingDTO.HostId;
                existingListing.PropertyTypeId = updateListingDTO.PropertyTypeId;
                existingListing.RoomTypeId = updateListingDTO.RoomTypeId;
                existingListing.Capacity = updateListingDTO.Capacity ?? existingListing.Capacity;
                existingListing.Bedrooms = updateListingDTO.Bedrooms;
                existingListing.Bathrooms = updateListingDTO.Bathrooms;
                existingListing.PricePerNight = updateListingDTO.PricePerNight;
                existingListing.ServiceFee = updateListingDTO.ServiceFee ?? existingListing.ServiceFee;
                existingListing.AddressLine1 = updateListingDTO.AddressLine1 ?? existingListing.AddressLine1;
                existingListing.AddressLine2 = updateListingDTO.AddressLine2 ?? existingListing.AddressLine2;
                existingListing.City = updateListingDTO.City ?? existingListing.City;
                existingListing.State = updateListingDTO.State ?? existingListing.State;
                existingListing.Country = updateListingDTO.Country ?? existingListing.Country;
                existingListing.PostalCode = updateListingDTO.PostalCode ?? existingListing.PostalCode;
                existingListing.InstantBooking = updateListingDTO.InstantBooking ?? existingListing.InstantBooking;
                existingListing.MinNights = updateListingDTO.MinNights ?? existingListing.MinNights;
                existingListing.MaxNights = updateListingDTO.MaxNights;
                existingListing.CancellationPolicyId = updateListingDTO.CancellationPolicyId ?? existingListing.CancellationPolicyId;
                existingListing.IsActive = updateListingDTO.IsActive ?? existingListing.IsActive;

                await _irepo.UpdateAsync(existingListing);

                return Ok(existingListing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}


//        // GET: api/listings - Get listings with filters
//        [HttpGet]
//        public async Task<IActionResult> GetListings([FromQuery] string city, [FromQuery] string country, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
//        {
//            var listingsQuery = _context.Listings.Include(l => l.Host)
//                                                  .Include(l => l.PropertyType)
//                                                  .Include(l => l.RoomType)
//                                                  .Include(l => l.Currency)
//                                                  .AsQueryable();

//            if (!string.IsNullOrEmpty(city))
//            {
//                listingsQuery = listingsQuery.Where(l => l.City.Contains(city));
//            }

//            if (!string.IsNullOrEmpty(country))
//            {
//                listingsQuery = listingsQuery.Where(l => l.Country.Contains(country));
//            }

//            if (minPrice.HasValue)
//            {
//                listingsQuery = listingsQuery.Where(l => l.PricePerNight >= minPrice);
//            }

//            if (maxPrice.HasValue)
//            {
//                listingsQuery = listingsQuery.Where(l => l.PricePerNight <= maxPrice);
//            }

//            var listings = await listingsQuery.ToListAsync();
//            return Ok(listings);
//        }

//    }
//}
