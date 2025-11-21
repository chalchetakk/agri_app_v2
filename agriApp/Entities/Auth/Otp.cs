using System;

namespace agriApp.Entities.Auth
{
    public class Otp
    {
        public Guid OtpId { get; private set; }

        // FK to UserProfile
        public Guid UserId { get; private set; }
        public UserProfile User { get; private set; }   // optional, but helpful for EF Core

        // Hashed OTP value
        public string OtpHash { get; private set; } = default!;

        // Expiration timestamp
        public DateTime ExpireAt { get; private set; }

        // Usage tracking
        public bool IsUsed { get; private set; }
        public DateTime? UsedAt { get; private set; }

        // Manual timestamp (no ABP auditing)
        public DateTime CreatedAt { get; private set; }


        // ------------------------------
        // Constructor – OTP creation
        // ------------------------------
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


        // Required by EF Core
        private Otp() { }


        // ------------------------------
        // Domain Methods
        // ------------------------------

        // Mark OTP as used
        public void MarkAsUsed()
        {
            if (IsUsed)
                throw new InvalidOperationException("OTP is already used.");

            if (IsExpired())
                throw new InvalidOperationException("Cannot use expired OTP.");

            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }

        // Check if OTP is expired
        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpireAt;
        }
    }
}
