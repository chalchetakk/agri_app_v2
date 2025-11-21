using agriApp.Data;
using agriApp.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AgriDbContext _db;
        private readonly IOtpService _otpService;
        private readonly ITokenService _tokenService;

        public AuthService(
            AgriDbContext db,
            IOtpService otpService,
            ITokenService tokenService)
        {
            _db = db;
            _otpService = otpService;
            _tokenService = tokenService;
        }

        public async Task<LoginResult> LoginWithOtpAsync(
            string mobileNumber,
            string otp,
            string deviceInfo,
            string ipAddress)
        {
            // STEP 1 — Verify OTP
            var isValidOtp = await _otpService.VerifyOtpAsync(mobileNumber, otp);
            if (!isValidOtp)
                throw new Exception("Invalid or expired OTP.");

            // STEP 2 — Load User
            var user = await _db.UserProfiles
                .FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);

            if (user == null)
                throw new Exception("User not found after OTP verification.");

            // Mark verified
            user.Verify();
            _db.UserProfiles.Update(user);

            // STEP 3 — Login Activity
            var loginEntry = new LoginActivity(
                userId: user.UserProfileId,
                isSuccessful: true,
                loginTime: DateTime.UtcNow,
                failureReason: null,
                ipAddress: ipAddress ?? "unknown-ip",
                deviceInfo: deviceInfo ?? "unknown-device"
            );

            _db.LoginActivities.Add(loginEntry);

            // STEP 4 — Generate Access Token
            var accessToken = _tokenService.GenerateAccessToken(
                user,
                deviceInfo ?? "unknown-device",
                ipAddress ?? "unknown-ip"
            );

            // Extract JTI
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(accessToken);
            var jti = jwt.Id;

            // STEP 5 — Refresh Token
            var refreshToken = await _tokenService.GenerateAndStoreRefreshTokenAsync(
                user.UserProfileId,
                deviceInfo ?? "unknown-device",
                ipAddress ?? "unknown-ip",
                jti
            );

            await _db.SaveChangesAsync();

            // STEP 6 — Return response
            return new LoginResult
            {
                UserId = user.UserProfileId,
                MobileNumber = user.MobileNumber,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = jwt.ValidTo,
                IsVerified = user.IsVerified
            };
        }
    }

    public class LoginResult
    {
        public Guid UserId { get; set; }
        public string MobileNumber { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public bool IsVerified { get; set; }
    }
}
