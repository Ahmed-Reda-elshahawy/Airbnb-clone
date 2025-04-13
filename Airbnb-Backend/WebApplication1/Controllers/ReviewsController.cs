using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        // POST: api/reviews
        [HttpPost]
        [Authorize]
        public IActionResult CreateReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            review.ReviewerId = userId;

            // Additional validation - check if user has actually stayed at this listing
            // You might want to add this check in the repository

            _reviewRepository.Create(review);
            _reviewRepository.Save();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        // GET: api/listings/{id}/reviews
        [HttpGet("~/api/listings/{id}/reviews")]
        public IActionResult GetReviewsForListing(Guid id)
        {
            var reviews = _reviewRepository.GetReviewsForListing(id);
            return Ok(reviews);
        }

        // GET: api/users/{id}/reviews
        [HttpGet("~/api/users/{id}/reviews")]
        [Authorize]
        public IActionResult GetReviewsForUser(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            if (id != userId)
                return Forbid();

            var reviews = _reviewRepository.GetReviewsForUser(id);
            return Ok(reviews);
        }

        // PUT: api/reviews/{id}
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateReview(Guid id, [FromBody] Review review)
        {
            if (id != review.Id)
                return BadRequest();

            var existingReview = _reviewRepository.GetById(id);
            if (existingReview == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            if (existingReview.ReviewerId != userId)
                return Forbid();

            _reviewRepository.UpdateAsync(review);
            _reviewRepository.Save();

            return NoContent();
        }

        // DELETE: api/reviews/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteReview(Guid id)
        {
            var review = _reviewRepository.GetById(id);
            if (review == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            if (review.ReviewerId != userId)
                return Forbid();

            _reviewRepository.Delete(review);
            _reviewRepository.Save();

            return NoContent();
        }

        // POST: api/reviews/{id}/host-reply
        [HttpPost("{id}/host-reply")]
        [Authorize]
        public IActionResult AddHostReply(Guid id, [FromBody] string reply)
        {
            var review = _reviewRepository.GetById(id);
            if (review == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            if (review.HostId != userId)
                return Forbid();

            if (!_reviewRepository.AddHostReply(id, reply))
                return BadRequest("Failed to add host reply");

            return Ok(new { Message = "Host reply added successfully" });
        }

        // Helper method
        private IActionResult GetReview(Guid id)
        {
            var review = _reviewRepository.GetById(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }
    }
}

