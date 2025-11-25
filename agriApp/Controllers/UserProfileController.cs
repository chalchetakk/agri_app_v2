using agriApp.Data;
using agriApp.Entities.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using agriApp.Services.Auth;
using agriApp.Services.Roles;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("user")]
    [Authorize]   // 🔒 All endpoints require AccessToken
    public class UserProfileController : ControllerBase
    {
        private readonly AgriDbContext _db;
        private readonly IOtpService _otpService;
        private readonly IRoleService _roleService;

        public UserProfileController(AgriDbContext db, IOtpService otpService)
        {
            _db = db;
            _otpService = otpService;
        }

        // ----------------------------------------------------------
        // 1️⃣ GET PROFILE
        // ----------------------------------------------------------
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            // var userIdClaim = User.FindFirst("sub")?.Value;
            // var userIdClaim = User.Identity?.Name;
            // var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
Console.WriteLine("IsAuthenticated: " + User.Identity?.IsAuthenticated);
// Console.WriteLine("Sub: " + User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
Console.WriteLine("Claims:");
foreach (var c in User.Claims)
{
    Console.WriteLine($"{c.Type}: {c.Value}");
}

            if (userIdClaim == null)
                return Unauthorized(new { message = "Invalid user." });

            var userId = Guid.Parse(userIdClaim);

            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.UserProfileId == userId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(new
            {
                userId = user.UserProfileId,
                mobileNumber = user.MobileNumber,
                preferredLanguage = user.PreferredLanguage,
                isVerified = user.IsVerified,
                lastLoginAt = user.LastLoginAt,
                createdAt = user.CreationTime,
                updatedAt = user.LastModificationTime
            });
        }

        // ----------------------------------------------------------
        // 2️⃣ UPDATE LANGUAGE
        // ----------------------------------------------------------
        [HttpPut("update-language")]
        public async Task<IActionResult> UpdateLanguage([FromBody] UpdateLanguageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Language))
                return BadRequest(new { message = "Language is required." });

            var userIdClaim = User.FindFirst("sub")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.UserProfileId == userId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            user.SetPreferredLanguage(request.Language);

            _db.UserProfiles.Update(user);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Language updated successfully." });
        }

        // ----------------------------------------------------------
        // 3️⃣ START MOBILE CHANGE (send OTP)
        // ----------------------------------------------------------
        [HttpPost("change-mobile/start")]
        public async Task<IActionResult> StartChangeMobile([FromBody] StartChangeMobileRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NewMobile))
                return BadRequest(new { message = "New mobile number is required." });

// 1. Get UserId from the JWT claim (this is required since this is an authorized endpoint)
    var userIdClaim = User.FindFirst("sub")?.Value;
    if (userIdClaim == null)
        return Unauthorized(); // Should not happen if [Authorize] is used

    var userId = Guid.Parse(userIdClaim);
    // 2. Load the current UserProfile
    var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.UserProfileId == userId);
    if (user == null)
        return NotFound(new { message = "Logged-in user not found." });

// Check if new number already exists
var exists = await _db.UserProfiles.AnyAsync(u => u.MobileNumber == request.NewMobile);
if (exists)
    return BadRequest(new { message = "Mobile number already in use." });


            // Send OTP for verification
            await _otpService.SendOtpAsync(request.NewMobile,userId.ToString(),allowCreateUser:false);

            return Ok(new { message = "OTP sent to new mobile number." });
        }

        // ----------------------------------------------------------
        // 4️⃣ CONFIRM MOBILE CHANGE (verify OTP + update)
        // ----------------------------------------------------------
        [HttpPost("change-mobile/confirm")]
        public async Task<IActionResult> ConfirmChangeMobile([FromBody] ConfirmMobileChangeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NewMobile) ||
                string.IsNullOrWhiteSpace(request.Otp))
            {
                return BadRequest(new { message = "Mobile and OTP are required." });
            }

            var userIdClaim = User.FindFirst("sub")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.UserProfileId == userId);
            if (user == null)
                return NotFound(new { message = "User not found." });
// Check if new number already exists
var exists = await _db.UserProfiles.AnyAsync(u => u.MobileNumber == request.NewMobile);
if (exists)
    return BadRequest(new { message = "Mobile number already in use." });

            // Verify OTP for new number
            var otpOk = await _otpService.VerifyOtpAsync(request.NewMobile, request.Otp);
            if (!otpOk)
                return BadRequest(new { message = "Invalid or expired OTP." });




            // Actually update mobile
            user.UpdateMobileNumber(request.NewMobile);

            _db.UserProfiles.Update(user);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Mobile number updated successfully." });
        }

        // ----------------------------------------------------------
// GET /roles/status
// ----------------------------------------------------------
[HttpGet("/roles/status")]
[Authorize]
public async Task<IActionResult> GetRoleStatus()
{
    var userIdClaim = User.FindFirst("sub")?.Value;
    if (userIdClaim == null)
        return Unauthorized(new { message = "Invalid user." });

    var userId = Guid.Parse(userIdClaim);

    var status = await _roleService.GetRoleStatusAsync(userId);

    return Ok(new
    {
        isFarmer = status.IsFarmer,
        isBuyer = status.IsBuyer,
        isSeller = status.IsSeller,
        isMandiOfficial = status.IsMandiOfficial
    });
}

    }





    // ----------------------------------------------------------
    // DTOs
    // ----------------------------------------------------------

    public class UpdateLanguageRequest
    {
        public string? Language { get; set; }
    }

    public class StartChangeMobileRequest
    {
        public string? NewMobile { get; set; }
    }

    public class ConfirmMobileChangeRequest
    {
        public string? NewMobile { get; set; }
        public string? Otp { get; set; }
    }

    public class RoleStatusDto
    {
        public bool IsFarmer { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsSeller { get; set; }
        public bool IsMandiOfficial { get; set; }
    }
}
