using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly AirbnbDBContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(AirbnbDBContext context , IMapper mapper) : base(context ,mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<Review> GetReviewsForListing(Guid listingId)
        {
            return _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Booking)
                .Include(r => r.Host)
                .Where(r => r.ListingId == listingId)
                .ToList();
        }

        public IEnumerable<Review> GetReviewsForUser(Guid userId)
        {
            return _context.Reviews
                .Include(r => r.Listing)
                .Include(r => r.Booking)
                .Include(r => r.Host)
                .Where(r => r.ReviewerId == userId || r.HostId == userId)
                .ToList();
        }

        public bool AddHostReply(Guid reviewId, string reply)
        {
            var review = _context.Reviews.Find(reviewId);
            if (review == null)
                return false;

            review.HostReply = reply;
            review.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public Review GetById(Guid id)
        {
            return _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Host)
                .Include(r => r.Booking)
                .FirstOrDefault(r => r.Id == id);
        }
    }
}
