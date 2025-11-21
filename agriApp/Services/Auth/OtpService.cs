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
        public async Task SendOtpAsync(string mobileNumber)
        {
            if (string.IsNullOrWhiteSpace(mobileNumber) || mobileNumber.Length != 10)
                throw new Exception("Invalid mobile number.");

            // Step 1 — Check if user exists
            var user = await _dbContext.UserProfiles
                .FirstOrDefaultAsync(x => x.MobileNumber == mobileNumber);

            // Step 2 — Create user if not exists
            if (user == null)
            {
                user = new UserProfile(mobileNumber);
                _dbContext.UserProfiles.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            // Step 3 — Generate 4-digit OTP
            var random = new Random();
            var otp = random.Next(1000, 9999).ToString();

            // Step 4 — Hash OTP
            var otpHash = HashOtp(otp);

            // Step 5 — Create OTP entry using domain constructor
            var otpEntity = new Otp(
                userId: user.UserProfileId,
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
