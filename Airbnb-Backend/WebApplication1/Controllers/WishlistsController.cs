using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS.WishList;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class WishlistsController : ControllerBase
    {

        private readonly IWishListRepository wishlistRepo;

        public WishlistsController(IWishListRepository _wishlistRepo)
        {
            wishlistRepo = _wishlistRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<WishlistDto>>> GetWishlists()
        {
            var userId = GetCurrentUserId();
            var wishlists = await wishlistRepo.GetUserWishlistsAsync(userId);
            return Ok(wishlists);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<WishlistDetailDto>> GetWishlist(Guid id)
        {
            try
            {
                var userId = User.Identity.IsAuthenticated ? GetCurrentUserId() : Guid.Empty;
                var wishlist = await wishlistRepo.GetWishlistByIdAsync(id, userId);
                return Ok(wishlist);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost]
        public async Task<ActionResult<WishlistDto>> CreateWishlist([FromBody] CreateWishlistDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var wishlist = await wishlistRepo.CreateWishlistAsync(userId, dto.Name, dto.IsPublic);
                return CreatedAtAction(nameof(GetWishlist), new { id = wishlist.Id }, wishlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WishlistDto>> UpdateWishlist(Guid id, [FromBody] UpdateWishlistDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var wishlist = await wishlistRepo.UpdateWishlistAsync(id, userId, dto.Name, dto.IsPublic);
                return Ok(wishlist);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWishlist(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await wishlistRepo.DeleteWishlistAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost("{wishlistId}/items")]
        public async Task<ActionResult<WishlistItemDto>> AddItemToWishlist(Guid wishlistId, [FromBody] AddWishlistItemDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var item = await wishlistRepo.AddItemToWishlistAsync(wishlistId, userId, dto.ListingId);
                return CreatedAtAction(nameof(GetWishlist), new { id = wishlistId }, item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{wishlistId}/items/{itemId}")]
        public async Task<ActionResult> RemoveItemFromWishlist(Guid wishlistId, Guid itemId)
        {
            try
            {
                var userId = GetCurrentUserId();
                await wishlistRepo.RemoveItemFromWishlistAsync(wishlistId, itemId, userId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        private Guid GetCurrentUserId()
        {
            return Guid.Parse("339574b6-c010-4cca-a489-8d74f390bb1b");//User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}

