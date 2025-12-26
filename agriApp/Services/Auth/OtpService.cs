using agriApp.Data;
using agriApp.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace agriApp.Services.Auth
{
    public class OtpService : IOtpService
    {
        private readonly AgriDbContext _dbContext;
        private readonly IConfiguration _config;

        public OtpService(AgriDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        // -------------------------------------------------------
        // SEND OTP
        // -------------------------------------------------------
      
// Assuming necessary using statements for Otp, UserProfile, and DbContext are here

public async Task SendOtpAsync(string mobileNumber, string userPreviousId = null, bool allowCreateUser = true)
{
    if (string.IsNullOrWhiteSpace(mobileNumber) || mobileNumber.Length != 10)
        throw new Exception("Invalid mobile number.");

    // Step 1 — Check if user exists for the given mobile number
    var user = await _dbContext.UserProfiles
        .FirstOrDefaultAsync(x => x.MobileNumber == mobileNumber);

    // Determine the GUID to link the OTP to.
    Guid targetUserId;

    // Step 2 — Handle User Existence and determine targetUserId
    if (user == null)
    {
        if (allowCreateUser)
        {
            // Case 1: User not found, creation allowed (Registration/Login)
            var newUser = new UserProfile(mobileNumber);
            _dbContext.UserProfiles.Add(newUser);
            await _dbContext.SaveChangesAsync();
            targetUserId = newUser.UserProfileId;
            // IMPORTANT: If we are here, 'user' is still null, but 'newUser' exists. 
            // We set the targetUserId and proceed.
        }
        else
        {
            // Case 2: User not found, creation disallowed (Change Mobile - new number)
            // We must rely on the logged-in user's ID passed via userPreviousId.
            if (string.IsNullOrWhiteSpace(userPreviousId) || !Guid.TryParse(userPreviousId, out targetUserId))
            {
                // If no valid ID is provided, we can't link the OTP.
                throw new Exception("Operation requires an existing User ID for verification.");
            }
            // If parsing succeeds, targetUserId is set.
        }
    }
    else
    {
        // Case 3: User found (Login or existing number validation)
        targetUserId = user.UserProfileId;
    }

    // Step 3 — Generate 4-digit OTP
    // var random = new Random();
    // var otp = random.Next(1000, 9999).ToString();
    var otp = "1234";

    // Step 4 — Hash OTP
    var otpHash = HashOtp(otp); // Assuming HashOtp is defined in the service

    // Step 5 — Create OTP entry using domain constructor
    // Note: otpEntity is declared once here and is accessible for saving. (Fixes CS0103)
    var otpEntity = new Otp(
        userId: targetUserId, // Now uses the determined GUID, safely (Fixes CS1061)
        otpHash: otpHash,
        expireAt: DateTime.UtcNow.AddMinutes(5)
    );

    _dbContext.Otps.Add(otpEntity);
    await _dbContext.SaveChangesAsync();

    // Step 6 — Send SMS (mock here)
    Console.WriteLine($"[DEV] OTP for {mobileNumber}: {otp}");
}

        // -------------------------------------------------------
        // VERIFY OTP
        // -------------------------------------------------------
        public async Task<bool> VerifyOtpAsync(string mobileNumber, string otp)
        {
            var user = await _dbContext.UserProfiles
                .FirstOrDefaultAsync(x => x.MobileNumber == mobileNumber);

            if (user == null)
                return false;

            var otpEntity = await _dbContext.Otps
                .Where(o => o.UserId == user.UserProfileId && !o.IsUsed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otpEntity == null)
                return false;

            if (otpEntity.IsExpired())
                return false;

            if (otpEntity.OtpHash != HashOtp(otp))
                return false;

            // Mark OTP used (domain method)
            otpEntity.MarkAsUsed();

            // Mark user verified (domain method)
            user.Verify();

            await _dbContext.SaveChangesAsync();

            return true;
        }

        // -------------------------------------------------------
        // HELPER — HASH OTP
        // -------------------------------------------------------
        private string HashOtp(string otp)
        {
            var key = _config["OtpSecretKey"] ?? "DefaultHardOtpSecret_key123";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));
            return Convert.ToBase64String(hash);
        }
    }
}
