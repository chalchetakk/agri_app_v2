using System.Collections.Generic;
using agriApp.Entities.Lots;
using agriApp.Entities.Auctions;

namespace agriApp.Entities.Market
{
    public class Mandi
    {
        public int MandiId { get; set; }  // PK
        public string MandiName { get; set; } = default!;
        public string Location { get; set; } = default!;

        public List<PreRegisteredLot> Lots { get; set; } = new();

        // Navigation to officials
        public List<agriApp.Entities.Stakeholders.MandiOfficial> Officials { get; set; } = new();

        // ⭐ NEW — Navigation to Auctions
        public List<Auction> Auctions { get; set; } = new();
    }
}
