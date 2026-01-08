using System.Collections.Generic;

namespace agriApp.Entities.Lookups
{
    public class MandiCategory
    {
        public long MandiCategoryId { get; set; }   // PK

        public string CategoryName { get; set; } = default!;
 public string Description { get; set; } = default!; // Full meaning
        public bool IsActive { get; set; } = true;

        // 🔒 Do NOT add navigation to Mandis yet (Phase 1 rule)
    }
}
