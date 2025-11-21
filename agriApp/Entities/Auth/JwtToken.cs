using System;

namespace agriApp.Entities.Auth
{
    public class JwtToken
    {
        public Guid JwtTokenId { get; set; }

        // FK → UserProfile
        public Guid UserId { get; set; }
        public UserProfile? User { get; set; }   // nullable navigation property

        // Access Token (short-lived)
        public string AccessTokenJti { get; set; } = default!;
        public DateTime AccessTokenIssuedAt { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }

        // Refresh Token (long-lived)
        public string RefreshTokenHash { get; set; } = default!;
        public DateTime RefreshTokenIssuedAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }

        // Token revocation
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Token rotation — link to next token in chain
        public Guid? ReplacedByTokenId { get; set; }

        // Metadata
        public string DeviceInfo { get; set; } = default!;
        public string IpAddress { get; set; } = default!;


        // EF Core requires public parameterless constructor
        public JwtToken() { }


        // Custom constructor used by TokenService
        public JwtToken(
            Guid userId,
            string accessTokenJti,
            DateTime accessTokenIssuedAt,
            DateTime accessTokenExpiresAt,
            string refreshTokenHash,
            DateTime refreshTokenIssuedAt,
            DateTime refreshTokenExpiresAt,
            string deviceInfo,
            string ipAddress)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.");

            if (string.IsNullOrWhiteSpace(accessTokenJti))
                throw new ArgumentException("AccessTokenJti cannot be empty.");

            if (string.IsNullOrWhiteSpace(refreshTokenHash))
                throw new ArgumentException("RefreshTokenHash cannot be empty.");

            if (accessTokenExpiresAt <= accessTokenIssuedAt)
                throw new ArgumentException("Access token expiry must be after issue time.");

            if (refreshTokenExpiresAt <= refreshTokenIssuedAt)
                throw new ArgumentException("Refresh token expiry must be after issue time.");

            JwtTokenId = Guid.NewGuid();

            UserId = userId;
            AccessTokenJti = accessTokenJti;
            AccessTokenIssuedAt = accessTokenIssuedAt;
            AccessTokenExpiresAt = accessTokenExpiresAt;

            RefreshTokenHash = refreshTokenHash;
            RefreshTokenIssuedAt = refreshTokenIssuedAt;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;

            DeviceInfo = deviceInfo;
            IpAddress = ipAddress;

            IsRevoked = false;
        }


        // -----------------------------
        // Domain Methods
        // -----------------------------
        public void Revoke(string? reason = null)
        {
            if (IsRevoked)
                return;

            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }

        public void SetReplacedBy(Guid newTokenId)
        {
            if (newTokenId == Guid.Empty)
                throw new ArgumentException("Invalid new token ID.");

            ReplacedByTokenId = newTokenId;
        }

        public bool IsRefreshExpired()
        {
            return DateTime.UtcNow > RefreshTokenExpiresAt;
        }

        public bool IsAccessExpired()
        {
            return DateTime.UtcNow > AccessTokenExpiresAt;
        }
    }
}
