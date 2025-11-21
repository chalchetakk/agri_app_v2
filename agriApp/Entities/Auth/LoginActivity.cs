using System;

namespace agriApp.Entities.Auth
{
    public class LoginActivity
    {
        public Guid ActivityId { get; set; }

        // FK to UserProfile
        public Guid UserId { get; set; }
        public UserProfile? User { get; set; }

        // Timestamp of login attempt
        public DateTime LoginTime { get; set; }

        // Success or failure
        public bool IsSuccessful { get; set; }

        // Reason (null if successful)
        public string? FailureReason { get; set; }

        // Optional metadata
        public string? IpAddress { get; set; }
        public string? DeviceInfo { get; set; }


        // EF Core requires public parameterless constructor
        public LoginActivity() { }


        // Constructor used by services
        public LoginActivity(
            Guid userId,
            bool isSuccessful,
            DateTime loginTime,
            string? failureReason = null,
            string? ipAddress = null,
            string? deviceInfo = null)
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
    }
}
