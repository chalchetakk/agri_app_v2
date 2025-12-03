using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using agriApp.Data;
using agriApp.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace agriApp.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly AgriDbContext _db;
        private readonly IConfiguration _config;

        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenMinutes;
        private readonly int _refreshTokenDays;

        public TokenService(AgriDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;

            _jwtSecret = _config["JwtSettings:SecretKey"] 
                ?? throw new ArgumentNullException("JwtSettings:SecretKey");

            _issuer = _config["JwtSettings:Issuer"] ?? "agriApp";
            _audience = _config["JwtSettings:Audience"] ?? "agriAppClients";

            _accessTokenMinutes =
                int.TryParse(_config["JwtSettings:AccessTokenMinutes"], out var m) ? m : 30;

            _refreshTokenDays =
                int.TryParse(_config["JwtSettings:RefreshTokenDays"], out var d) ? d : 7;
        }

        // ------------------------------------------------------------
        // Generate Access Token (HS256)
        // ------------------------------------------------------------
        public string GenerateAccessToken(UserProfile user, string deviceInfo, string ipAddress, string? roleCode)
{
    var jti = Guid.NewGuid().ToString();
    var now = DateTime.UtcNow;
    var expires = now.AddMinutes(_accessTokenMinutes);

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserProfileId.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, jti),
        new Claim("mobile", user.MobileNumber),
        new Claim("isVerified", user.IsVerified.ToString())
    };

    if (!string.IsNullOrWhiteSpace(roleCode))
    {
        // THE MOST IMPORTANT FIX!
        claims.Add(new Claim(ClaimTypes.Role, roleCode)); 
         claims.Add(new Claim("role", roleCode));           // Optional: For frontend use
    }

    var token = new JwtSecurityToken(
        issuer: _issuer,
        audience: _audience,
        claims: claims,
        notBefore: now,
        expires: expires,
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}


        // ------------------------------------------------------------
        // Create and Store Refresh Token
        // ------------------------------------------------------------
        public async Task<string> GenerateAndStoreRefreshTokenAsync(
            Guid userId,
            string deviceInfo,
            string ipAddress,
            string accessTokenJti)
        {
            var rawRefreshToken = GenerateSecureRandomToken(64);

            var secretKey = _config["RefreshTokenSecretKey"] 
                ?? _config["JwtSettings:SecretKey"];

            var refreshHash = HashWithHmac(rawRefreshToken, secretKey);

            var now = DateTime.UtcNow;

            var tokenRow = new JwtToken(
                userId: userId,
                accessTokenJti: accessTokenJti,
                accessTokenIssuedAt: now,
                accessTokenExpiresAt: now.AddMinutes(_accessTokenMinutes),
                refreshTokenHash: refreshHash,
                refreshTokenIssuedAt: now,
                refreshTokenExpiresAt: now.AddDays(_refreshTokenDays),
                deviceInfo: deviceInfo ?? "",
                ipAddress: ipAddress ?? ""
            );

            _db.JwtTokens.Add(tokenRow);
            await _db.SaveChangesAsync();

            return rawRefreshToken;
        }

        // ------------------------------------------------------------
        // Validate Refresh Token
        // ------------------------------------------------------------
        public async Task<JwtToken?> ValidateRefreshTokenAsync(string refreshTokenRaw)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenRaw))
                return null;

            var secretKey = _config["RefreshTokenSecretKey"] 
                ?? _config["JwtSettings:SecretKey"];

            var hash = HashWithHmac(refreshTokenRaw, secretKey);

            var row = await _db.JwtTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.RefreshTokenHash == hash);

            if (row == null) return null;
            if (row.IsRevoked) return null;
            if (DateTime.UtcNow > row.RefreshTokenExpiresAt) return null;

            return row;
        }

        // ------------------------------------------------------------
        // Revoke Refresh Token
        // ------------------------------------------------------------
        public async Task RevokeRefreshTokenAsync(Guid jwtTokenId)
        {
            var row = await _db.JwtTokens.FirstOrDefaultAsync(x => x.JwtTokenId == jwtTokenId);
            if (row == null) return;

            if (!row.IsRevoked)
            {
                row.Revoke();
                await _db.SaveChangesAsync();
            }
        }

        // ------------------------------------------------------------
        // Refresh Token Rotation
        // ------------------------------------------------------------
        public async Task<(string accessToken, string refreshToken)> RotateRefreshTokenAsync(string oldRefreshTokenRaw)
        {
            var oldRow = await ValidateRefreshTokenAsync(oldRefreshTokenRaw);
            if (oldRow == null)
                throw new Exception("Invalid or expired refresh token.");

            var user = oldRow.User ??
                       await _db.UserProfiles.FirstAsync(u => u.UserProfileId == oldRow.UserId);

            // Reuse detection (attack)
            if (oldRow.IsRevoked && oldRow.ReplacedByTokenId != null)
            {
                var allTokens = await _db.JwtTokens
                    .Where(x => x.UserId == user.UserProfileId)
                    .ToListAsync();

                foreach (var t in allTokens)
                    t.Revoke();

                await _db.SaveChangesAsync();

                throw new Exception("Refresh token reuse detected. All sessions revoked.");
            }
// Fetch mandi official role of this user (needed for Role claim)
var official = await _db.MandiOfficials
    .Include(m => m.Role)
    .FirstOrDefaultAsync(m => m.UserId == user.UserProfileId);

string? roleCode = official?.Role?.RoleCode;  // OFFICER / APPROVER / MANAGER

            // Generate new access token
            var newAccessToken = GenerateAccessToken(user, oldRow.DeviceInfo, oldRow.IpAddress,roleCode);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(newAccessToken);
            var newJti = jwt.Id;

            // Generate new refresh token
            var newRawRefresh = GenerateSecureRandomToken(64);
            var secretKey = _config["RefreshTokenSecretKey"] ?? _config["JwtSettings:SecretKey"];
            var newHash = HashWithHmac(newRawRefresh, secretKey);

            var now = DateTime.UtcNow;

            var newRow = new JwtToken(
                userId: user.UserProfileId,
                accessTokenJti: newJti,
                accessTokenIssuedAt: now,
                accessTokenExpiresAt: now.AddMinutes(_accessTokenMinutes),
                refreshTokenHash: newHash,
                refreshTokenIssuedAt: now,
                refreshTokenExpiresAt: now.AddDays(_refreshTokenDays),
                deviceInfo: oldRow.DeviceInfo,
                ipAddress: oldRow.IpAddress
            );

            _db.JwtTokens.Add(newRow);

            // Revoke old token
            oldRow.Revoke();
            oldRow.SetReplacedBy(newRow.JwtTokenId);

            await _db.SaveChangesAsync();

            return (newAccessToken, newRawRefresh);
        }

        // ------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------
        private static string GenerateSecureRandomToken(int byteLength)
        {
            var bytes = new byte[byteLength];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string HashWithHmac(string input, string secretKey)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}
