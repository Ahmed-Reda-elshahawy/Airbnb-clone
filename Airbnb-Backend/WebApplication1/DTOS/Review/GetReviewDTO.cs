namespace WebApplication1.DTOS.Review
{
    public class GetReviewDTO
    {
        public Guid Id { get; set; }

        public Guid BookingId { get; set; }

        public Guid ReviewerId { get; set; }

        public Guid HostId { get; set; }

        public Guid ListingId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public string HostReply { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? CleanlinessRating { get; set; }

        public int? AccuracyRating { get; set; }

        public int? CommunicationRating { get; set; }

        public int? LocationRating { get; set; }

        public int? CheckInRating { get; set; }

        public int? ValueRating { get; set; }
    }
}