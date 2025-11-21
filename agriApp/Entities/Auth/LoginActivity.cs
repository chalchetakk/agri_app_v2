using System;

namespace agriApp.Entities.Auth
{
    public class LoginActivity
    {
        public Guid ActivityId { get; private set; }

        // FK to UserProfile
        public Guid UserId { get; private set; }
        public UserProfile? User { get; private set; }  // optional navigation property

        // Timestamp of login attempt
        public DateTime LoginTime { get; private set; }

        // Success or failure
        public bool IsSuccessful { get; private set; }

        // Null when successful; contains reason when failed
        public string? FailureReason { get; private set; }

        // Optional metadata
        public string? IpAddress { get; private set; }
        public string? DeviceInfo { get; private set; }


        // ---------------------------------------
        // Constructor – Create a login activity
        // ---------------------------------------

        public LoginActivity(
            Guid userId,
            bool isSuccessful,
            DateTime loginTime,
            string failureReason = null,
            string ipAddress = null,
            string deviceInfo = null)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.");

            if (isSuccessful && failureReason != null)
                throw new ArgumentException("FailureReason must be null for successful logins.");

            if (!isSuccessful && string.IsNullOrWhiteSpace(failureReason))
                throw new ArgumentException("FailureReason is required for failed login attempts.");

            ActivityId = Guid.NewGuid();

            UserId = userId;
            IsSuccessful = isSuccessful;
            LoginTime = loginTime;

            FailureReason = failureReason;
            IpAddress = ipAddress;
            DeviceInfo = deviceInfo;
        }


        // Required for EF Core
        private LoginActivity() { }
    }
}
