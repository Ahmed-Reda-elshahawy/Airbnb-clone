using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UserService: IUser
    {

        private readonly IWebHostEnvironment _environment; // Used to get web root path for file uploads
        private readonly AirbnbDBContext _context; // Database context

        // Constructor with dependency injection
        public UserService(IWebHostEnvironment environment, AirbnbDBContext context)
        {
            _environment = environment;
            _context = context;
        }

        

        // Implementation of profile picture saving
        public string SaveProfilePicture(Stream imageStream, string fileName, Guid userId)
        {
            // Create directory for uploads if it doesn't exist
            // Organizing uploads in folders helps with file management
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles");
            Directory.CreateDirectory(uploadsFolder); // Create directory if doesn't exist

            // Generate unique filename to prevent collisions and file overwrites
            // Using userId and timestamp ensures uniqueness
            var uniqueFileName = $"{userId}_{DateTime.UtcNow.Ticks}_{Path.GetFileName(fileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file using a FileStream
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageStream.CopyTo(fileStream); // Copy uploaded file to destination
            }

            // Return relative URL (not physical path) for database storage and client usage
            return $"/uploads/profiles/{uniqueFileName}";
        }

    }
}
