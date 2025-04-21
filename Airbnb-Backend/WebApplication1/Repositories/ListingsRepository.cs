using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOS.Listing;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ListingsRepository : GenericRepository<Listing>, IListing
    {
        #region Dependency Injection
        private readonly AirbnbDBContext context;
        private readonly List<string> includeProperties =
        [
            "ListingPhotos",
            "ListingAmenities",
            "ListingAmenities.Amenity",
            "Reviews",
            "Reviews.Reviewer",
            "Host",
            "CancellationPolicy",
        ];
        public ListingsRepository(AirbnbDBContext _context, IMapper _mapper, IHttpContextAccessor httpContextAccessor) : base(_context, _mapper, httpContextAccessor)
        {
            context = _context;
        }
        #endregion

        #region Get Methods
        public async Task<Listing> GetListingWithDetailsbyId(Guid id)
        {
            return await GetByIDAsync(id, includeProperties);
        }
        public async Task<IEnumerable<Listing>> GetListingsWithDetails(Dictionary<string, string> queryParams)
        {
            return await GetAllAsync(queryParams, includeProperties);
        }
        #endregion

        #region Add Amenities to Listing
        public async Task<List<ListingAmenity>> AddAmenitiesToListing(Guid listingId, List<Guid> amenityIds)
        {
            var listing = await context.Listings
                                       .Include(l => l.ListingAmenities)
                                       .FirstOrDefaultAsync(l => l.Id == listingId);
            if (listing == null)
            {
                return null;
            }
            var amenities = await context.Amenities
                                         .Where(a => amenityIds.Contains(a.Id))
                                         .ToListAsync();
            if (amenities.Count != amenityIds.Count)
            {
                return null;
            }
            var newlyaddedAmenity = new List<ListingAmenity>();
            foreach (var amenity in amenities)
            {
                if (!listing.ListingAmenities.Any(al => al.AmenityId == amenity.Id))
                {
                    var newListingAmenity = new ListingAmenity
                    {
                        ListingId = listingId,
                        AmenityId = amenity.Id
                    };
                    listing.ListingAmenities.Add(newListingAmenity);
                    newlyaddedAmenity.Add(newListingAmenity);

                }

            }
            await context.SaveChangesAsync();
            return newlyaddedAmenity;
        }
        #endregion

        #region Remove Amenities from Listing
        public async Task<bool> RemoveAmenityFromListing(Guid listingId, Guid amenityId)
        {
            var listingAmenity = await context.ListingAmenities
                                               .FirstOrDefaultAsync(la => la.ListingId == listingId && la.AmenityId == amenityId);

            if (listingAmenity == null)
            {
                return false;
            }

            context.ListingAmenities.Remove(listingAmenity);
            await context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Create Empty Listing
        public async Task<Listing> CreateEmptyListing()
        {
            var currentUser = GetCurrentUserId();
            var User = await context.Users
                .Include(u => u.Currency)
                .FirstOrDefaultAsync(u => u.Id == currentUser);
            var listing = new Listing
            {
                HostId = currentUser,
                PropertyTypeId = 1,
                RoomTypeId = 1,
                AddressLine1 = string.Empty,
                City = string.Empty,
                State = string.Empty,
                Country = string.Empty,
                PostalCode = string.Empty,
                CurrencyId = User.CurrencyId,
                Title = string.Empty,
                Description = string.Empty,
            };
            await CreateAsync(listing);
            return listing;
        }
        #endregion

        #region Update Listing
        [HttpPut("{id}")]
        public async Task<Listing> UpdateListing(Guid id, UpdateListingDTO dto)
        {
            try
            {
                var updatedListing = await UpdateAsync<Listing, UpdateListingDTO>(id, dto);

                if (updatedListing == null)
                    return null;

                var fullListing = await GetByIDAsync(id);
                if (fullListing.IsActive)
                {
                    fullListing.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();
                }

                if (dto.AmenityIds != null && dto.AmenityIds.Count > 0)
                {
                    var addedAmenities = await AddAmenitiesToListing(id, dto.AmenityIds);
                    if (addedAmenities == null)
                        return null; 
                }
                return fullListing;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the listing", ex);
            }
        }
        #endregion

        #region Update verification Status
        public async Task<bool> UpdateVerificationStatusAsync(Guid listingId, int statusId)
        {
            var listing = await GetByIDAsync(listingId);
            if (listing == null) return false;

            listing.VerificationStatusId = statusId;
            context.Listings.Update(listing);
            await context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}