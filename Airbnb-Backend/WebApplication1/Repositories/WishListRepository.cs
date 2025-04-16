using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOS.WishList;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class WishListRepository : IWishListRepository
    {
        private readonly AirbnbDBContext context;

        public WishListRepository(AirbnbDBContext _context)
        {
            context = _context;
        }

        public async Task<List<WishlistDto>> GetUserWishlistsAsync(Guid userId)
        {
            return await context.Wishlists
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.CreatedAt)
                .Select(w => new WishlistDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    IsPublic = w.IsPublic ?? false,
                    CreatedAt = w.CreatedAt ?? DateTime.UtcNow,
                    ItemCount = w.WishlistItems.Count
                })
                .ToListAsync();
        }

        public async Task<WishlistDetailDto> GetWishlistByIdAsync(Guid wishlistId, Guid userId)
        {
            var wishlist = await context.Wishlists
                .Include(w => w.WishlistItems)
                    .ThenInclude(wi => wi.Listing)
                .FirstOrDefaultAsync(w => w.Id == wishlistId);

            if (wishlist == null)
            {
                throw new KeyNotFoundException("Wishlist not found");
            }

            // Check if user can view this wishlist
            if (wishlist.UserId != userId && !(wishlist.IsPublic ?? false))
            {
                throw new UnauthorizedAccessException("You cannot view this wishlist");
            }

            return new WishlistDetailDto
            {
                Id = wishlist.Id,
                Name = wishlist.Name,
                IsPublic = wishlist.IsPublic ?? false,
                CreatedAt = wishlist.CreatedAt ?? DateTime.UtcNow,
                Items = wishlist.WishlistItems.Select(wi => new WishlistItemDto
                {
                    Id = wi.Id,
                    ListingId = wi.ListingId,
                    ListingTitle = wi.Listing.Title,
                    ListingPhotos = wi.Listing.ListingPhotos,
                    ListingPricePerNight = wi.Listing.PricePerNight,
                    AddedAt = wi.AddedAt ?? DateTime.UtcNow
                }).ToList()
            };
        }

        public async Task<WishlistDto> CreateWishlistAsync(Guid userId, string name, bool isPublic)
        {
            var wishlist = new Wishlist
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                IsPublic = isPublic,
                CreatedAt = DateTime.UtcNow
            };

            context.Wishlists.Add(wishlist);
            await context.SaveChangesAsync();

            return new WishlistDto
            {
                Id = wishlist.Id,
                Name = wishlist.Name,
                IsPublic = wishlist.IsPublic ?? false,
                CreatedAt = wishlist.CreatedAt ?? DateTime.UtcNow,
                ItemCount = 0
            };
        }

        public async Task<WishlistDto> UpdateWishlistAsync(Guid wishlistId, Guid userId, string name, bool isPublic)
        {
            var wishlist = await context.Wishlists
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.Id == wishlistId);

            if (wishlist == null)
            {
                throw new KeyNotFoundException("Wishlist not found");
            }

            if (wishlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("You cannot update this wishlist");
            }

            wishlist.Name = name;
            wishlist.IsPublic = isPublic;

            await context.SaveChangesAsync();

            return new WishlistDto
            {
                Id = wishlist.Id,
                Name = wishlist.Name,
                IsPublic = wishlist.IsPublic ?? false,
                CreatedAt = wishlist.CreatedAt ?? DateTime.UtcNow,
                ItemCount = wishlist.WishlistItems.Count
            };
        }

        public async Task DeleteWishlistAsync(Guid wishlistId, Guid userId)
        {
            var wishlist = await context.Wishlists
                .FirstOrDefaultAsync(w => w.Id == wishlistId);

            if (wishlist == null)
            {
                throw new KeyNotFoundException("Wishlist not found");
            }

            if (wishlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("You cannot delete this wishlist");
            }

            context.Wishlists.Remove(wishlist);
            await context.SaveChangesAsync();
        }

        public async Task<WishlistItemDto> AddItemToWishlistAsync(Guid wishlistId, Guid userId, Guid listingId)
        {
            var wishlist = await context.Wishlists
                .FirstOrDefaultAsync(w => w.Id == wishlistId);

            if (wishlist == null)
            {
                throw new KeyNotFoundException("Wishlist not found");
            }

            if (wishlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("You cannot modify this wishlist");
            }

            var listing = await context.Listings
                .FirstOrDefaultAsync(l => l.Id == listingId);

            if (listing == null)
            {
                throw new KeyNotFoundException("Listing not found");
            }

            // Check if item already exists in wishlist
            var existingItem = await context.WishlistItems
                .FirstOrDefaultAsync(wi => wi.WishlistId == wishlistId && wi.ListingId == listingId);

            if (existingItem != null)
            {
                throw new InvalidOperationException("This item is already in your wishlist");
            }

            var wishlistItem = new WishlistItem
            {
                Id = Guid.NewGuid(),
                WishlistId = wishlistId,
                ListingId = listingId,
                AddedAt = DateTime.UtcNow
            };

            context.WishlistItems.Add(wishlistItem);
            await context.SaveChangesAsync();

            return new WishlistItemDto
            {
                Id = wishlistItem.Id,
                ListingId = listing.Id,
                ListingTitle = listing.Title,
                ListingPhotos = listing.ListingPhotos,
                ListingPricePerNight = listing.PricePerNight,
                AddedAt = wishlistItem.AddedAt ?? DateTime.UtcNow
            };
        }

        public async Task RemoveItemFromWishlistAsync(Guid wishlistId, Guid itemId, Guid userId)
        {
            var wishlist = await context.Wishlists
                .FirstOrDefaultAsync(w => w.Id == wishlistId);

            if (wishlist == null)
            {
                throw new KeyNotFoundException("Wishlist not found");
            }

            if (wishlist.UserId != userId)
            {
                throw new UnauthorizedAccessException("You cannot modify this wishlist");
            }

            var wishlistItem = await context.WishlistItems
                .FirstOrDefaultAsync(wi => wi.Id == itemId && wi.WishlistId == wishlistId);

            if (wishlistItem == null)
            {
                throw new KeyNotFoundException("Wishlist item not found");
            }

            context.WishlistItems.Remove(wishlistItem);
            await context.SaveChangesAsync();
        }
    }

}
