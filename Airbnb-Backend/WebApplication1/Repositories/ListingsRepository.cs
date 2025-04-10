using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using static WebApplication1.Repositories.ListingsRepository;

namespace WebApplication1.Repositories
{
    public class ListingsRepository : GenericRepository<Listing>
    {
        #region Dependency Injection
        private readonly AirbnbDBContext context;
        private readonly IMapper mapper;
        private readonly List<string> includeProperties = new List<string>
        {
            "ListingPhotos",
            "ListingAmenities",
            "ListingAmenities.Amenity"
        };
        public ListingsRepository(AirbnbDBContext _context, IMapper _mapper) : base(_context, _mapper)
        {
            context = _context;
            mapper = _mapper;
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
    }
}