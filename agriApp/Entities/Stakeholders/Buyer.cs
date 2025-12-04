using System;
using System.Collections.Generic;
using agriApp.Entities.Auth;
using agriApp.Entities.Lots;

namespace agriApp.Entities.Stakeholders
{
    public class Buyer
    {
        public Guid BuyerId { get; set; }

        public Guid UserId { get; set; }
        public UserProfile? User { get; set; }

        public string BuyerName { get; set; } = default!;
        public string? BusinessId { get; set; }
        public string BusinessName { get; set; } = default!;
        public string? Location { get; set; }
        public string? ProfilePhotoUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<BuyerInterestedCrop> InterestedCrops { get; set; } = new();
public List<BuyerInterestLot> BuyerInterestLots { get; set; } = new();

        public Buyer() {}

        public Buyer(Guid userId, string buyerName, string businessName, string? businessId, string? location, string? profilePhotoUrl)
        {
            BuyerId = Guid.NewGuid();
            UserId = userId;

            BuyerName = buyerName;
            BusinessName = businessName;
            BusinessId = businessId;
            Location = location;
            ProfilePhotoUrl = profilePhotoUrl;

            CreatedAt = DateTime.UtcNow;
        }

        public void Update()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
