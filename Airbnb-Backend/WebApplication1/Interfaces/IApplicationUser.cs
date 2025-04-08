using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IApplicationUser
    {
        // Interface for user-related operations - using interfaces allows for better testability and dependency injection

        // Method to save profile picture - defined in interface to allow for mocking in tests
        // Returns the URL of the saved image
        string SaveProfilePicture(Stream imageStream, string fileName, Guid userId);
    }
}
