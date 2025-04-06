using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Core.Entities
{
    public class User : BaseEntity
    {

        public string Email { get; set; }

        public string Password { get; set; }

        public string First_Name { get; set; }

        public string Last_Nmae { get; set; }

        public string? Phone_Number { get; set; }

        public DateOnly? Date_Of_Birth { get; set; }

        public string? Profile_Picture_Url { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime Updated_At { get; set; }

        public bool Is_Host { get; set; } = false;

        public string? Bio { get; set; }

        public ICollection<Listing>? Listings { get; set; } = new HashSet<Listing>();


    }
}
