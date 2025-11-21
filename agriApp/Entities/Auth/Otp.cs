using System;

namespace agriApp.Entities.Auth
{
    public class Otp
    {
        public Guid OtpId { get; set; }

        // FK to UserProfile
        public Guid UserId { get; set; }
        public UserProfile? User { get; set; }   // nullable navigation property

        // Hashed OTP value
        public string OtpHash { get; set; } = default!;

        // Expiration timestamp
        public DateTime ExpireAt { get; set; }

        // Usage tracking
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }

        // Manual timestamp
        public DateTime CreatedAt { get; set; }


        // EF Core needs public parameterless constructor
        public Otp() { }


        // Custom constructor used by OtpService
        public Otp(Guid userId, string otpHash, DateTime expireAt)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.");

            if (string.IsNullOrWhiteSpace(otpHash))
                throw new ArgumentException("OtpHash cannot be empty.");

            if (expireAt <= DateTime.UtcNow)
                throw new ArgumentException("ExpireAt must be in the future.");

            OtpId = Guid.NewGuid();
            UserId = userId;
            OtpHash = otpHash;
            ExpireAt = expireAt;

            IsUsed = false;
            CreatedAt = DateTime.UtcNow;
        }


        // ------------------------------
        // Domain Methods
        // ------------------------------

        public void MarkAsUsed()
        {
            if (IsUsed)
                throw new InvalidOperationException("OTP is already used.");

            if (IsExpired())
                throw new InvalidOperationException("Cannot use expired OTP.");

            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpireAt;
        }
    }
}
