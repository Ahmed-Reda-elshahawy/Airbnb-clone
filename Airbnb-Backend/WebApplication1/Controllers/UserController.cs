using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // Contains UserManager and other identity-related functionality
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOS.ApplicationUser;
using WebApplication1.Interfaces;
using WebApplication1.Models;
//using WebApplication1.Services; // Contains interfaces for our services



namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/users")]
    //[Authorize] // Requires authenticated users for all endpoints (unless overridden)
    public class UserController : ControllerBase
    {
        private readonly IRepository<ApplicationUser> irepo;
        private readonly UserManager<ApplicationUser> userManager; // Identity's UserManager
        private readonly IUser userService;
        private readonly IVerification verificationService;
        private readonly IMapper mapper;
<<<<<<< HEAD
        //private readonly IPhotoHandler photoHandler;
=======
>>>>>>> 8a71405 (hmm)

        public UserController(
            IRepository<ApplicationUser> _irepo,
            UserManager<ApplicationUser> _userManager, // Injected to manage user entities
            IUser _userService,
            IVerification _verificationService,
<<<<<<< HEAD
            IMapper _mapper,
            IPhotoHandler _photoHandler)
=======
            IMapper _mapper)
>>>>>>> 8a71405 (hmm)
        {
            irepo = _irepo;
            userManager = _userManager;
            userService = _userService;
            verificationService = _verificationService;
            mapper = _mapper;
<<<<<<< HEAD
            //photoHandler = _photoHandler;
=======
>>>>>>> 8a71405 (hmm)

        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers([FromQuery] Dictionary<string, string> queryParams)
        {
            var users = await irepo.GetAllAsync(queryParams);
            var usersDto = mapper.Map<List<ApplicationUserDto>>(users);
            return Ok(usersDto);
        }
<<<<<<< HEAD

        [HttpGet("me")]
        public async Task<ActionResult> GetCurrentUserProfile()
        {
            var user = await userService.GetCurrentUserAsync();
=======
        [HttpGet("me")]
        public async Task<ActionResult> GetCurrentUserProfile()
        {
            var userId = GetCurrentUserId();//GetCurrentUserId();
            var user = await irepo.GetByIDAsync(userId);
>>>>>>> 8a71405 (hmm)

            if (user == null)
                return NotFound();

            return Ok(user);
        }

<<<<<<< HEAD
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetUserProfile(Guid userId)
        {
            var user = await userService.GetUserAsync(userId);
=======
        [HttpPut("me")]
        public async Task<ActionResult<ApplicationUser>> UpdateCurrentUserProfile([FromBody] ApplicationUserDto updatedUser) // [FromBody] binds JSON in request body to the parameter
        {
            var userId = Guid.Parse("2108206a-5b14-4cc4-a092-d1c22e094694");//GetCurrentUserId();
            var user = await irepo.GetByIDAsync(userId);
            if (user == null)
                return NotFound(); // Return 404 if user not found

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Bio = updatedUser.Bio;
            user.DateOfBirth = updatedUser.DateOfBirth;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await irepo.UpdateAsync<ApplicationUser, ApplicationUserDto>(userId, updatedUser);
            if (result == null)
                return BadRequest("update failed");

            return NoContent();
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Override the [Authorize] attribute to allow unauthenticated access for this endpoint
        public async Task<ActionResult> GetUserProfile(Guid id)
        {
            var userId = Guid.Parse("2108206a-5b14-4cc4-a092-d1c22e094694");//GetCurrentUserId();
            var user = await irepo.GetByIDAsync(userId);
>>>>>>> 8a71405 (hmm)

            if (user == null)
                return NotFound();

            var publicProfile = mapper.Map<ApplicationUser, GetApplicationUserDto>(user);
            return Ok(publicProfile);
        }

<<<<<<< HEAD
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(PostApplicationUserDto applicationUserDTO)
        {

            applicationUserDTO.Id = Guid.NewGuid().ToString();
            applicationUserDTO.CreatedAt = DateTime.UtcNow;
            applicationUserDTO.IsVerified = false;
            var User = mapper.Map<ApplicationUser>(applicationUserDTO);
            await irepo.CreateAsync(User);
            irepo.Save();
            return CreatedAtAction("GetApplicationUser", User
                , applicationUserDTO);
        }

        // DELETE: api/ApplicationUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(Guid id)
        {
            var applicationUser = await userService.GetUserAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            await irepo.DeleteAsync(id);
            irepo.Save();

            return NoContent();
        }


        [HttpPut("me")]
        public async Task<ActionResult<ApplicationUser>> UpdateCurrentUserProfile([FromBody] UpdateApplicationUserDto updatedUser) // [FromBody] binds JSON in request body to the parameter
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userService.GetCurrentUserAsync();
            if (user == null)
                return NotFound();
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Bio = updatedUser.Bio;
            user.DateOfBirth = updatedUser.DateOfBirth;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await irepo.UpdateAsync<ApplicationUser, UpdateApplicationUserDto>(user.Id, updatedUser);
            if (result == null)
                return BadRequest("update failed");

            return Ok(user);
        }

=======
>>>>>>> 8a71405 (hmm)
        // POST /api/users/me/profile-picture - Upload profile picture
        [HttpPost("me/profile-picture")]
        public async Task<ActionResult> UploadProfilePicture(IFormFile file)
        {
<<<<<<< HEAD
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded"); 

            if (!userService.IsValidImageFile(file)) 
                return BadRequest("Invalid file type. Only image files are allowed.");

            if (file.Length > 5 * 1024 * 1024) 
                return BadRequest("File size exceeds the limit (5MB)");

            var user = await userService.GetCurrentUserAsync();
=======
            if (file == null || file.Length == 0) // Validate that file exists and is not empty
                return BadRequest("No file uploaded"); // Return 400 if validation fails

            if (!IsValidImageFile(file)) // Validate file type using helper method
                return BadRequest("Invalid file type. Only image files are allowed.");

            if (file.Length > 5 * 1024 * 1024) // Check file size (5MB limit) - prevents DOS attacks and excessive resource usage
                return BadRequest("File size exceeds the limit (5MB)");

            var userId = Guid.Parse("2108206a-5b14-4cc4-a092-d1c22e094694");//GetCurrentUserId();
            var user = await irepo.GetByIDAsync(userId);
>>>>>>> 8a71405 (hmm)

            if (user == null)
                return NotFound();

            try
            {
                using (var stream = file.OpenReadStream()) // Open file stream, and ensure it's disposed after use with 'using'
                {
<<<<<<< HEAD
                    string pictureUrl = userService.SaveProfilePicture(stream, file.FileName);
=======
                    // Save profile picture and get URL using service
                    string pictureUrl = userService.SaveProfilePicture(stream, file.FileName, userId);
>>>>>>> 8a71405 (hmm)

                    user.ProfilePictureUrl = pictureUrl;
                    user.UpdatedAt = DateTime.UtcNow;
                    var UpdateUserDto = mapper.Map<ApplicationUser, UpdateApplicationUserDto>(user);
<<<<<<< HEAD
                    var result = await irepo.UpdateAsync<ApplicationUser, UpdateApplicationUserDto>(user.Id, UpdateUserDto); // Save changes

                    if (result == null)
                        return BadRequest("Update failed"); 

                    return Ok(new { ProfilePictureUrl = pictureUrl });
=======
                    var result = await irepo.UpdateAsync<ApplicationUser, UpdateApplicationUserDto>(userId, UpdateUserDto); // Save changes

                    if (result == null)
                        return BadRequest("Update failed"); // Return 400 with errors if update failed

                    return Ok(new { ProfilePictureUrl = pictureUrl }); // Return 200 with picture URL
>>>>>>> 8a71405 (hmm)
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while uploading the profile picture"); // Return 500 for server errors
            }
        }

<<<<<<< HEAD
        //PUT /api/users/me/preferences - Update user preferences
        [HttpPut("me/preferences")]
        public async Task<ActionResult> UpdateUserPreferences([FromBody] UpdateApplicationUserPreferencesDto preferences) // Using custom class to receive only relevant data
        {
            var user = await userService.GetCurrentUserAsync();

            if (user == null)
                return NotFound();

            user.PreferredLanguage = preferences.PreferredLanguage;

            if (preferences.CurrencyId.HasValue) 
            {
                user.CurrencyId = preferences.CurrencyId;
            }

            user.UpdatedAt = DateTime.UtcNow; // Update timestamp
            var UpdateUserDto = mapper.Map<ApplicationUser, UpdateApplicationUserPreferencesDto>(user);
            var result = await irepo.UpdateAsync<ApplicationUser,UpdateApplicationUserPreferencesDto>(user.Id, UpdateUserDto); // Save changes

            if (result == null)
                return BadRequest("Update failed");

            return NoContent(); // 204 No Content on successful update
        }
=======
       // //PUT /api/users/me/preferences - Update user preferences
       //[HttpPut("me/preferences")] // HTTP PUT for updating preferences
       // public ActionResult UpdateUserPreferences([FromBody] UserPreferences preferences) // Using custom class to receive only relevant data
       // {
       //     var userId = Guid.Parse("2108206a-5b14-4cc4-a092-d1c22e094694");//GetCurrentUserId();
       //     var user = userManager.FindByIdAsync(userId.ToString()).Result;

       //     if (user == null)
       //         return NotFound();

       //     user.PreferredLanguage = preferences.Language; // Update language preference

       //     // Handle currency - only update if a value is provided
       //     if (preferences.CurrencyId.HasValue) // Check if currency ID has a value (not null)
       //     {
       //         user.CurrencyId = preferences.CurrencyId; // Update currency ID
       //     }

       //     user.UpdatedAt = DateTime.UtcNow; // Update timestamp

       //     var result = _userManager.Update(user); // Save changes

       //     if (!result.Succeeded)
       //         return BadRequest(result.Errors);

       //     return NoContent(); // 204 No Content on successful update
       // }
>>>>>>> 8a71405 (hmm)



        // GET /api/users/me/verification-status - Check verification status
        [HttpGet("me/verification-status")]
        public async Task<ActionResult> GetVerificationStatus()
        {
<<<<<<< HEAD
            var user = await userService.GetCurrentUserAsync();
            if (user == null)
                return NotFound();

            var status = verificationService.GetVerificationStatus(user.VerificationStatusId);

            return Ok(status);
        }

        // POST /api/users/me/verify - Submit verification documents
        [HttpPost("me/verify")]
        public async Task<ActionResult> SubmitVerificationDocuments()
        {
            var user = await userService.GetCurrentUserAsync();
=======
            var userId = Guid.Parse("2108206a-5b14-4cc4-a092-d1c22e094694");//GetCurrentUserId();
            var user = await irepo.GetByIDAsync(userId);
            //var user = userManager.FindByIdAsync(userId.ToString()).Result;

            if (user == null)
                return NotFound();

            // Get verification status using service - separating concerns
            var status = verificationService.GetVerificationStatus(user.VerificationStatusId);

            return Ok(status); // Return status
        }

        // POST /api/users/me/verify - Submit verification documents
        [HttpPost("me/verify")] // POST for creating new verification documents
        public async Task<ActionResult> SubmitVerificationDocuments()
        {
            var userId = Guid.Parse("2108206a-5b14-4cc4-a092-d1c22e094694");//GetCurrentUserId();
            var user = await irepo.GetByIDAsync(userId);
>>>>>>> 8a71405 (hmm)

            if (user == null)
                return NotFound();

<<<<<<< HEAD
            // Access files from the form directly - instead of using parameter binding 
            //as we might need to handle multiple files and form fields
            var files = Request.Form.Files; // Get files from form
            if (files == null || files.Count == 0)
                return BadRequest("No documents uploaded");

=======
            // Access files from the form directly - did this instead of parameter binding 
            // because we might need to handle multiple files and form fields
            var files = Request.Form.Files; // Get files from form
            if (files == null || files.Count == 0) // Validate files exist
                return BadRequest("No documents uploaded");

            // Get form fields
>>>>>>> 8a71405 (hmm)
            var documentType = Request.Form["documentType"].ToString(); // Get document type from form
            var additionalInfo = Request.Form["additionalInfo"].ToString(); // Get additional info from form

            try
            {
                // Submit documents using service
                bool success = verificationService.SubmitVerificationDocuments(
<<<<<<< HEAD
                    user.Id,
=======
                    userId,
>>>>>>> 8a71405 (hmm)
                    files.ToList(), // Convert to List for service
                    documentType,
                    additionalInfo);

                if (!success)
                    return BadRequest("Failed to submit verification documents");

                // Update user verification status to "In Progress"
                user.VerificationStatusId = verificationService.GetInProgressStatusId(); // Get status ID from service
                irepo.UpdateAsync(user); // Save changes

                return Ok(new { message = "Verification documents submitted successfully" }); // Return success message
            }
            catch (Exception ex)
            {
                // Log exception (would implement actual logging in production)
                return StatusCode(500, "An error occurred while submitting verification documents"); // 500 for server errors
            }
<<<<<<< HEAD
        }      

    }
}
=======
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


#region reused code

//[Route("api/[controller]")]
//[ApiController]
//public class ApplicationUsersController : ControllerBase
//{
//    private readonly AirbnbDBContext _context;

//    public ApplicationUsersController(AirbnbDBContext context)
//    {
//        _context = context;
//    }

//    // GET: api/ApplicationUsers
//    [HttpGet]


//    // GET: api/ApplicationUsers/5
//    [HttpGet("{id}")]
//    public async Task<ActionResult<ApplicationUser>> GetApplicationUser(Guid id)
//    {
//        var applicationUser = await _context.Users.FindAsync(id);

//        if (applicationUser == null)
//        {
//            return NotFound();
//        }

//        return applicationUser;
//    }

//    // PUT: api/ApplicationUsers/5
//    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//    [HttpPut("{id}")]
//    public async Task<IActionResult> PutApplicationUser(Guid id, ApplicationUser applicationUser)
//    {
//        if (id != applicationUser.Id)
//        {
//            return BadRequest();
//        }

//        _context.Entry(applicationUser).State = EntityState.Modified;

//        try
//        {
//            await _context.SaveChangesAsync();
//        }
//        catch (DbUpdateConcurrencyException)
//        {
//            if (!ApplicationUserExists(id))
//            {
//                return NotFound();
//            }
//            else
//            {
//                throw;
//            }
//        }

//        return NoContent();
//    }

//    // POST: api/ApplicationUsers
//    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//    [HttpPost]
//    public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser applicationUser)
//    {
//        _context.Users.Add(applicationUser);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
//    }

//    // DELETE: api/ApplicationUsers/5
//    [HttpDelete("{id}")]
//    public async Task<IActionResult> DeleteApplicationUser(Guid id)
//    {
//        var applicationUser = await _context.Users.FindAsync(id);
//        if (applicationUser == null)
//        {
//            return NotFound();
//        }

//        _context.Users.Remove(applicationUser);
//        await _context.SaveChangesAsync();

//        return NoContent();
//    }

//    private bool ApplicationUserExists(Guid id)
//    {
//        return _context.Users.Any(e => e.Id == id);
//    }
//}
#endregion
>>>>>>> 8a71405 (hmm)
