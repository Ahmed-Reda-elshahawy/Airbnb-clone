using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Core.Entities
{
    public class Listing : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }

        public string RoomType { get; set; }

        public int Capacity { get; set; }
        
        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public decimal Price_Per_Night { get; set; }

        public decimal Service_Fee { get; set; } = 0;

        public string Address_Line1 { get; set; }

        public string? Address_Line2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime Updated_At { get; set; }

        public int Min_Nights { get; set; } = 1;

        public int? Max_Nights { get; set; }

        public string? Cancellation_Policy { get; set; }

        public decimal? Average_Rating { get; set; }

        public int Review_Count { get; set; } = 0;

        public bool IsActive { get; set; } = true;


        public int Host_Id { get; set; }
        public User Host { get; set; }







    }
}
