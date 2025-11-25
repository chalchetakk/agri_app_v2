using System;
using System.Collections.Generic;

namespace agriApp.Entities.Stakeholders
{
    public class Seller
    {
        public Guid SellerId { get; set; }   // GUID primary key

        public Guid UserId { get; set; }     // FK to UserProfile
        public string SellerName { get; set; } = default!;
        public string? BusinessName { get; set; }
        public string Location { get; set; } = default!;
        public string? ProfilePhotoUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<SellerInterestedCrop> InterestedCrops { get; set; } = new();

        public Seller() { }

        // constructor used by service
        public Seller(Guid userId, string sellerName, string? businessName, string location, string? photoUrl)
        {
            SellerId = Guid.NewGuid();
            UserId = userId;
            SellerName = sellerName;
            BusinessName = businessName;
            Location = location;
            ProfilePhotoUrl = photoUrl;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
