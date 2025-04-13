using WebApplication1.DTOS.Review;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IReview 
    {
        Task<Review> CreateReview(Guid bookingId, CreateReviewDTO dto);
        Task<bool> AddHostReplyAsync(Guid reviewId, Guid hostId, string replyMessage);
    }
}