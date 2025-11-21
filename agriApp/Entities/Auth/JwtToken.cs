using System;

namespace agriApp.Entities.Auth
{
    public class JwtToken
    {
        public Guid JwtTokenId { get; private set; }

        // FK → UserProfile
        public Guid UserId { get; private set; }
        public UserProfile User { get; private set; }

        // Access Token (short-lived)
        public string AccessTokenJti { get; private set; } = default!;
        public DateTime AccessTokenIssuedAt { get; private set; }
        public DateTime AccessTokenExpiresAt { get; private set; }

        // Refresh Token (long-lived)
        public string RefreshTokenHash { get; private set; } = default!;
        public DateTime RefreshTokenIssuedAt { get; private set; } = default!;
        public DateTime RefreshTokenExpiresAt { get; private set; }

        // Token revocation
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        // Token rotation — link to next token in chain
        public Guid? ReplacedByTokenId { get; private set; }

        // Metadata
        public string DeviceInfo { get; private set; }
        public string IpAddress { get; private set; }


        // -------------------------------------------------
        // Constructor — Create new token pair (Access+Refresh)
        // -------------------------------------------------
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


        // Required by EF Core
        private JwtToken() { }


        // -------------------------------------------------
        // Domain Methods
        // -------------------------------------------------

        // Revoke this refresh token
        public void Revoke(string reason = null)
        {
            if (IsRevoked)
                return;

            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }

        // Link to the new token issued during rotation
        public void SetReplacedBy(Guid newTokenId)
        {
            if (newTokenId == Guid.Empty)
                throw new ArgumentException("Invalid new token ID.");

            ReplacedByTokenId = newTokenId;
        }

        // Refresh token lifetime check
        public bool IsRefreshExpired()
        {
            return DateTime.UtcNow > RefreshTokenExpiresAt;
        }

        // Access token lifetime check
        public bool IsAccessExpired()
        {
            return DateTime.UtcNow > AccessTokenExpiresAt;
        }
    }
}
