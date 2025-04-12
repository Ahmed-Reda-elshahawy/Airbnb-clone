using WebApplication1.Models;

namespace WebApplication1.DTOS.ApplicationUser
{
    public class UpdateApplicationUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }

<<<<<<< HEAD
        public DateTime? DateOfBirth { get; set; }

=======
>>>>>>> 8a71405 (hmm)
        public string Bio { get; set; }

        public bool? IsHost { get; set; }

        public bool? IsVerified { get; set; }        
    }
}
