using System;
using System.Collections.Generic;
using agriApp.Entities.Auth;
using agriApp.Entities.Stakeholders;
using agriApp.Entities.Lots;

namespace agriApp.Entities.Stakeholders
{
    public class Farmer
    {
        public Guid FarmerId { get; set; }

        public Guid UserId { get; set; }
        public UserProfile? User { get; set; }

        public string FarmerName { get; set; } = default!;
        public string? Location { get; set; }
        public string? ProfilePhotoUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation collections
        public List<FarmerInterestedCrop> InterestedCrops { get; set; } = new();
        public List<FarmDetails> FarmDetails { get; set; } = new();
    public List<PreRegisteredLot> Lots { get; set; } = new();


        // EF needs empty constructor
        public Farmer() {}

        public Farmer(Guid userId, string farmerName, string? location, string? profilePhotoUrl)
        {
            FarmerId = Guid.NewGuid();
            UserId = userId;

            FarmerName = farmerName ?? throw new ArgumentException("FarmerName is required.");
            Location = location;
            ProfilePhotoUrl = profilePhotoUrl;

            CreatedAt = DateTime.UtcNow;
        }

        public void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
