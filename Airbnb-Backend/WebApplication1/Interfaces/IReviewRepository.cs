using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IReviewRepository : IRepository<Review> 
    {
        IEnumerable<Review> GetReviewsForListing(Guid listingId);
        IEnumerable<Review> GetReviewsForUser(Guid userId);
        bool AddHostReply(Guid reviewId, string reply);
        Review GetById(Guid id);
    }
}
