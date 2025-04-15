using Microsoft.AspNetCore.Identity;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UserRepository: IUser
    {

        private readonly IWebHostEnvironment _environment; // Used to get web root path for file uploads
        private readonly AirbnbDBContext _context;
        private readonly IRepository<ApplicationUser> irepo;
        //private readonly IdentityUser identityUser;
        public UserRepository(IWebHostEnvironment environment, AirbnbDBContext context, IRepository<ApplicationUser> _irepo)
        {
            _environment = environment;
            _context = context;
            irepo = _irepo;
            //identityUser = _identityuser;
        }

        private Guid GetCurrentUserId()
        {
            // Find the name identifier claim (contains user ID)
            //var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //    throw new InvalidOperationException("User ID claim not found"); // Throw exception if claim not found

            //return Guid.Parse(userIdClaim.Value); // Parse claim value to Guid
            return Guid.Parse("23d411dc-dc91-4c82-948a-f0f7c7f4b903"); // Parse claim value to Guid
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            var user = await irepo.GetByIDAsync(userId);
            return user;
        }
        public async Task<ApplicationUser> GetUserAsync(Guid userId)
        {
            var user = await irepo.GetByIDAsync(userId);
            return user;
        }

        public string SaveProfilePicture(Stream imageStream, string fileName)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageStream.CopyTo(fileStream);
            }

            return $"/uploads/profiles/{uniqueFileName}";
        }

        public bool IsValidImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                return false;

            return true;
        }
    }
}
