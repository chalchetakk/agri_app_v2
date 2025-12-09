using System;
using System.Collections.Generic;
using agriApp.Entities.Auth;
using agriApp.Entities.Lots;

namespace agriApp.Entities.Stakeholders
{
    public class Anchor
    {
        public Guid AnchorId { get; set; }

        public Guid UserId { get; set; }
        public UserProfile? User { get; set; }

        public string CompanyName { get; set; } = default!;
        public string RegistrationNumber { get; set; } = default!;
        public string CompanyAddress { get; set; } = default!;
        public string ContactPersonName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string ContactPersonNum { get; set; } = default!;

        public string? GSTNumber { get; set; }
        public int EstimatedFarmersNum { get; set; }
        public string? BusinessDescription { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Anchor() { }

        public Anchor(
            Guid userId,
            string companyName,
            string registrationNumber,
            string companyAddress,
            string contactPersonName,
            string email,
            string contactPersonNum,
            int estimatedFarmersNum,
            string? gstNumber = null,
            string? businessDescription = null)
        {
            AnchorId = Guid.NewGuid();
            UserId = userId;

            CompanyName = companyName;
            RegistrationNumber = registrationNumber;
            CompanyAddress = companyAddress;
            ContactPersonName = contactPersonName;
            Email = email;
            ContactPersonNum = contactPersonNum;

            EstimatedFarmersNum = estimatedFarmersNum;
            GSTNumber = gstNumber;
            BusinessDescription = businessDescription;

            CreatedAt = DateTime.UtcNow;
        }

        public void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
