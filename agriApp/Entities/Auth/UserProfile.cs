    using System;

    namespace agriApp.Entities.Auth
    {
        public class UserProfile
        {
            public Guid UserProfileId { get; set; }

            public string MobileNumber { get; set; } = default!;
  public string? UserName { get; private set; }   // ⭐ NEW
            public string PreferredLanguage { get; set; } = "en";

            public bool IsVerified { get; set; }

            public DateTime? LastLoginAt { get; set; }

            public DateTime CreationTime { get; set; }
            public DateTime? LastModificationTime { get; set; }


            // EF Core requires public parameterless constructor
            public UserProfile() { }


            // Constructor for creating user manually
            public UserProfile(string mobileNumber, string preferredLanguage = "en")
            {
                UserProfileId = Guid.NewGuid();

                ValidateMobileNumber(mobileNumber);
                MobileNumber = mobileNumber;
UserName = mobileNumber; 
                SetPreferredLanguage(preferredLanguage);

                IsVerified = false;
                CreationTime = DateTime.UtcNow;
            }


            // --------------------------
            // Helpers
            // --------------------------
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


            // --------------------------
            // Domain Methods
            // --------------------------

            // Rename to match AuthService usage
            public void Verify()
            {
                IsVerified = true;
                TouchModified();
            }

            public void UpdateMobileNumber(string newNumber)
            {
                ValidateMobileNumber(newNumber);

                if (newNumber == MobileNumber)
                    throw new ArgumentException("New mobile number cannot match the old number.");

                MobileNumber = newNumber;
                IsVerified = false;
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
            public void SetUserName(string name)
    {
        UserName = string.IsNullOrWhiteSpace(name)
        ? MobileNumber
        : name.Trim();
        LastModificationTime = DateTime.UtcNow;
    }
        }
    }
