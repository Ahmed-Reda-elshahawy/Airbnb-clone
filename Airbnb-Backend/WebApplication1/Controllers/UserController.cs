using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;// Needed for IFormFile when handling file uploads
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity; // Contains UserManager and other identity-related functionality
using System; // Basic system types like Guid, DateTime
using System.IO; // File and stream operations
using System.Linq; // LINQ operations
using WebApplication1.Models; // Contains ApplicationUser and other models
using WebApplication1.Repositories; // Contains services models
//using WebApplication1.Services; // Contains interfaces for our services

 

namespace WebApplication1.Controllers
{
    [ApiController] // Marks this class as an API controller, which enables routing, model binding, and other API-specific features
    [Route("api/users")] // Defines the base route for all endpoints in this controller
    //[Authorize] // Requires authenticated users for all endpoints (unless overridden)
    public class UserController : ControllerBase
    {
        private readonly IRepository<ApplicationUser> irepo;
        private readonly UserManager<ApplicationUser> _userManager; // Identity's UserManager to handle user operations
        private readonly IUser _userService; // Custom service for user-related functionality
        private readonly IVerification _verificationService; // Custom service for verification-related functionality

        // Constructor with dependency injection - ASP.NET Core's DI container will provide these instances
        public UserController(
            IRepository<ApplicationUser> _irepo,
            UserManager<ApplicationUser> userManager, // Injected to manage user entities
            IUser userService, // Injected for user operations
            IVerification verificationService) // Injected for verification operations
        {
            irepo = _irepo;
            _userManager = userManager;
            _userService = userService;
            _verificationService = verificationService;
        }


        [HttpGet("all")]
        public ActionResult<IEnumerable<ApplicationUser>> GetAll()
            {
                var users = irepo.GetAll();
                return Ok(users);
            }

        // GET /api/users/me - Get current user profile
        [HttpGet("me")] // HTTP GET on the "me" route
        public ActionResult GetCurrentUserProfile() // Return type is ActionResult for flexibility in response types
        {
            var userId = GetCurrentUserId(); // Extract user ID from claims
            var user = _userManager.FindByIdAsync(userId.ToString()).Result; //Use UserManager to find the user by ID and Use FindByIdAsync and ensure userId is converted to string

            if (user == null) // Check if user exists
                return NotFound(); // Return 404 if user not found

            return Ok(user); // Return 200 OK with user data
        }

        // PUT /api/users/me - Update current user profile
        [HttpPut("me")] // HTTP PUT on the "me" route - PUT is used for updating existing resources
        public void UpdateCurrentUserProfile([FromBody] ApplicationUser updatedUser) // [FromBody] binds JSON in request body to the parameter
        {
            var userId = GetCurrentUserId(); // Get current user's ID
            var user = _userManager.FindByIdAsync(userId.ToString()).Result; // Find the actual user in database

            //if (user == null)
            //    return NotFound(); // Return 404 if user not found

            // Only update specific allowed fields (security practice - don't blindly update all fields)
            user.FirstName = updatedUser.FirstName; // Update first name
            user.LastName = updatedUser.LastName; // Update last name
            user.Bio = updatedUser.Bio; // Update bio
            user.DateOfBirth = updatedUser.DateOfBirth; // Update date of birth
            user.UpdatedAt = DateTime.UtcNow; // Set updated timestamp to current time (using UTC for consistency)

            // Save changes using UserManager
            /*var result =*/
            irepo.Update(user);

            //if (!result.Succeeded) // Check if update was successful
            //    return BadRequest(result.Errors); // Return 400 with errors if update failed

            //return NoContent(); // Return 204 No Content on successful update (common REST practice)
        }

        // GET /api/users/{id} - Get specific user profile (public info)
        [HttpGet("{id}")] // HTTP GET with route parameter
        [AllowAnonymous] // Override the [Authorize] attribute to allow unauthenticated access for this endpoint
        public ActionResult GetUserProfile(Guid id) // Parameter matches the route parameter {id}
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result; // Find user by ID

            if (user == null)
                return NotFound(); // Return 404 if user not found

            // Return only public information - create anonymous object instead of exposing entire user entity
            // This is a simple alternative to using DTOs for controlling what data is exposed
            var publicProfile = new
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Bio = user.Bio,
                IsHost = user.IsHost,
                IsVerified = user.IsVerified
            };

            return Ok(publicProfile); // Return 200 OK with public profile data
        }

        // POST /api/users/me/profile-picture - Upload profile picture
        [HttpPost("me/profile-picture")] // HTTP POST for creating a new resource (profile picture)
        public /*ActionResult*/ void UploadProfilePicture(IFormFile file) // IFormFile for handling file uploads
        {
            //if (file == null || file.Length == 0) // Validate that file exists and is not empty
            //    return BadRequest("No file uploaded"); // Return 400 if validation fails

            //if (!IsValidImageFile(file)) // Validate file type using helper method
            //    return BadRequest("Invalid file type. Only image files are allowed.");

            //if (file.Length > 5 * 1024 * 1024) // Check file size (5MB limit) - prevents DOS attacks and excessive resource usage
            //    return BadRequest("File size exceeds the limit (5MB)");

            var userId = GetCurrentUserId(); // Get current user ID
            var user = _userManager.FindByIdAsync(userId.ToString()).Result; // Find user

            //if (user == null)
            //    return NotFound(); // Return 404 if user not found

            try // Use try-catch to handle file IO exceptions
            {
                using (var stream = file.OpenReadStream()) // Open file stream, and ensure it's disposed after use with 'using'
                {
                    // Save profile picture and get URL using service
                    string pictureUrl = _userService.SaveProfilePicture(stream, file.FileName, userId);

                    // Update user with new picture URL
                    user.ProfilePictureUrl = pictureUrl;
                    user.UpdatedAt = DateTime.UtcNow; // Update timestamp

                    /*var result =*/ 
                    irepo.Update(user); // Save changes

                    //if (!result.Succeeded)
                    //    return BadRequest(result.Errors); // Return 400 with errors if update failed

                    //return Ok(new { pictureUrl }); // Return 200 with picture URL
                }
            }
            catch (Exception ex) // Catch any exceptions during file operations
            {
                // Log exception (would add actual logging in production)
                //return StatusCode(500, "An error occurred while uploading the profile picture"); // Return 500 for server errors
            }
        }

        // PUT /api/users/me/preferences - Update user preferences
        //[HttpPut("me/preferences")] // HTTP PUT for updating preferences
        //public ActionResult UpdateUserPreferences([FromBody] UserPreferences preferences) // Using custom class to receive only relevant data
        //{
        //    var userId = GetCurrentUserId();
        //    var user = _userManager.FindByIdAsync(userId.ToString()).Result;

        //    if (user == null)
        //        return NotFound();

        //    user.PreferredLanguage = preferences.Language; // Update language preference

        //    // Handle currency - only update if a value is provided
        //    if (preferences.CurrencyId.HasValue) // Check if currency ID has a value (not null)
        //    {
        //        user.CurrencyId = preferences.CurrencyId; // Update currency ID
        //    }

        //    user.UpdatedAt = DateTime.UtcNow; // Update timestamp

        //    var result = _userManager.Update(user); // Save changes

        //    if (!result.Succeeded)
        //        return BadRequest(result.Errors);

        //    return NoContent(); // 204 No Content on successful update
        //}



        // GET /api/users/me/verification-status - Check verification status
        [HttpGet("me/verification-status")]
        public ActionResult GetVerificationStatus()
        {
            var userId = GetCurrentUserId();
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;

            if (user == null)
                return NotFound();

            // Get verification status using service - separating concerns
            var status = _verificationService.GetVerificationStatus(user.VerificationStatusId);

            return Ok(status); // Return status
        }

        // POST /api/users/me/verify - Submit verification documents
        [HttpPost("me/verify")] // POST for creating new verification documents
        public ActionResult SubmitVerificationDocuments()
        {
            var userId = GetCurrentUserId();
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;

            if (user == null)
                return NotFound();

            // Access files from the form directly - did this instead of parameter binding 
            // because we might need to handle multiple files and form fields
            var files = Request.Form.Files; // Get files from form
            if (files == null || files.Count == 0) // Validate files exist
                return BadRequest("No documents uploaded");

            // Get form fields
            var documentType = Request.Form["documentType"].ToString(); // Get document type from form
            var additionalInfo = Request.Form["additionalInfo"].ToString(); // Get additional info from form

            try
            {
                // Submit documents using service
                bool success = _verificationService.SubmitVerificationDocuments(
                    userId,
                    files.ToList(), // Convert to List for service
                    documentType,
                    additionalInfo);

                if (!success)
                    return BadRequest("Failed to submit verification documents");

                // Update user verification status to "In Progress"
                user.VerificationStatusId = _verificationService.GetInProgressStatusId(); // Get status ID from service
                irepo.Update(user); // Save changes

                return Ok(new { message = "Verification documents submitted successfully" }); // Return success message
            }
            catch (Exception ex)
            {
                // Log exception (would implement actual logging in production)
                return StatusCode(500, "An error occurred while submitting verification documents"); // 500 for server errors
            }
        }

        // Helper method to get current user ID from claims
        private Guid GetCurrentUserId()
        {
            // Find the name identifier claim (contains user ID)
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new InvalidOperationException("User ID claim not found"); // Throw exception if claim not found

            return Guid.Parse(userIdClaim.Value); // Parse claim value to Guid
        }

        // Helper method to validate image files
        private bool IsValidImageFile(IFormFile file)
        {
            // Define allowed extensions for security (prevent upload of executable files)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant(); // Get extension and convert to lowercase

            // Check if extension is in allowed list
            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                return false;

            return true;
        }
    }
}
