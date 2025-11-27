using System.Collections.Generic;

namespace agriApp.Entities.Market
{
    public class Mandi
    {
        public int MandiId { get; set; }  // PK (int)
        public string MandiName { get; set; } = default!;
        public string Location { get; set; } = default!;

        // Navigation
        public List<agriApp.Entities.Stakeholders.MandiOfficial> Officials { get; set; } = new();
    }
}
