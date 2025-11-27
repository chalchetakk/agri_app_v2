using System;
using System.Collections.Generic;

namespace agriApp.Entities.Stakeholders
{
    public class OfficialRole
    {
        public Guid OfficialRoleId { get; set; }
        public string OfficialRoleName { get; set; } = default!;
        public string RoleCode { get; set; } = default!;   // e.g., OFFICER, MANAGER, APPROVER

        // Navigation
        public List<MandiOfficial> MandiOfficials { get; set; } = new();
    }
}
