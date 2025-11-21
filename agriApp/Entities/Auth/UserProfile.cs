using System;

namespace agriApp.Entities.Auth
{
    public class UserProfile
    {
        public Guid UserProfileId { get; private set; }

        public string MobileNumber { get; private set; } = default!;

        public string PreferredLanguage { get; private set; } = default!;

        public bool IsVerified { get; private set; }

        public DateTime? LastLoginAt { get; private set; }

        public DateTime CreationTime { get; private set; }
        public DateTime? LastModificationTime { get; private set; }


        // ----------------------------
        // Constructor (User creation)
        // ----------------------------
        public UserProfile(string mobileNumber, string preferredLanguage = "en")
        {
            UserProfileId = Guid.NewGuid();

            ValidateMobileNumber(mobileNumber);
            MobileNumber = mobileNumber;

            SetPreferredLanguage(preferredLanguage);

            IsVerified = false;
            CreationTime = DateTime.UtcNow;
        }


        // Required by EF Core
        private UserProfile() { }


        // ----------------------------
        // PRIVATE helpers
        // ----------------------------
        private void ValidateMobileNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Mobile number cannot be empty.");

            if (number.Length != 10 || !long.TryParse(number, out _))
                throw new ArgumentException("Mobile number must be a 10-digit numeric value.");
        }

        private void TouchModified()
        {
            LastModificationTime = DateTime.UtcNow;
        }


        // ----------------------------
        // PUBLIC domain methods
        // ----------------------------

        // Step 4 of verification flow — OTP already verified
        public void UpdateMobileNumber(string newNumber)
        {
            ValidateMobileNumber(newNumber);

            if (newNumber == MobileNumber)
                throw new ArgumentException("New mobile number cannot be the same as the current one.");

            MobileNumber = newNumber;

            // After new number verification is complete
            IsVerified = false;

            TouchModified();
        }

        public void MarkVerified()
        {
            IsVerified = true;
            TouchModified();
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
            TouchModified();
        }

        public void SetPreferredLanguage(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                throw new ArgumentException("Preferred language cannot be empty.");

            PreferredLanguage = lang;
            TouchModified();
        }
    }
}
