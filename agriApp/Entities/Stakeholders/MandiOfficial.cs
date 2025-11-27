using System;
using agriApp.Entities.Market;
using agriApp.Entities.Auth;
using agriApp.Entities.Stakeholders;

namespace agriApp.Entities.Stakeholders
{
    public class MandiOfficial
    {
        public Guid OfficialId { get; set; }

        public string OfficialName { get; set; } = default!;
        public string EmployeeId { get; set; } = default!;
        public string Email { get; set; } = default!;

        // FK → Mandis (int)
        public int MandiId { get; set; }
        public Mandi? Mandi { get; set; }

        // FK → OfficialRole (Guid)
        public Guid OfficialRoleId { get; set; }
        public OfficialRole? Role { get; set; }

        // FK → Users
        public Guid UserId { get; set; }
        public UserProfile? User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
