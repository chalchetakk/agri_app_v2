using agriApp.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(
            IOtpService otpService,
            IAuthService authService,
            ITokenService tokenService)
        {
            _otpService = otpService;
            _authService = authService;
            _tokenService = tokenService;
        }

        // -------------------------------------------------------
        // SEND OTP
        // -------------------------------------------------------
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.MobileNumber))
                return BadRequest(new { message = "Mobile number is required." });

            try
            {
                await _otpService.SendOtpAsync(request.MobileNumber);
                return Ok(new { message = "OTP sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -------------------------------------------------------
        // VERIFY OTP → Login + Tokens
        // -------------------------------------------------------
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.MobileNumber) ||
                string.IsNullOrWhiteSpace(request.Otp))
            {
                return BadRequest(new { message = "Mobile number and OTP are required." });
            }

            try
            {
                var result = await _authService.LoginWithOtpAsync(
                    request.MobileNumber,
                    request.Otp,
                    request.DeviceInfo ?? "unknown-device",
                    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
                );

                return Ok(new
                {
                    userId = result.UserId,
                    mobileNumber = result.MobileNumber,
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken,
                    expiresIn = result.ExpiresIn,
                    isVerified = result.IsVerified
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -------------------------------------------------------
        // REFRESH TOKEN
        // -------------------------------------------------------
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { message = "Refresh token is required." });

            try
            {
                var (newAccessToken, newRefreshToken) =
                    await _tokenService.RotateRefreshTokenAsync(request.RefreshToken);

                return Ok(new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -------------------------------------------------------
        // LOGOUT
        // -------------------------------------------------------
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { message = "Refresh token is required." });

            try
            {
                var tokenRow = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);

                if (tokenRow == null)
                    return BadRequest(new { message = "Invalid refresh token." });

                await _tokenService.RevokeRefreshTokenAsync(tokenRow.JwtTokenId);

                return Ok(new { message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // -------------------------------------------------------
    // DTOs
    // -------------------------------------------------------
    public class SendOtpRequest
    {
        public string? MobileNumber { get; set; }
    }

    public class VerifyOtpRequest
    {
        public string? MobileNumber { get; set; }
        public string? Otp { get; set; }
        public string? DeviceInfo { get; set; }
    }

    public class RefreshRequest
    {
        public string? RefreshToken { get; set; }
    }

    public class LogoutRequest
    {
        public string? RefreshToken { get; set; }
    }
}
