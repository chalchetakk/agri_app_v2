using System.Collections.Generic;
using agriApp.Entities.Lots;
using agriApp.Entities.Auctions;
using agriApp.Entities.Stakeholders;

namespace agriApp.Entities.Market
{
    public class Mandi
    {
        public int MandiId { get; set; }  // PK

        public string MandiName { get; set; } = default!;

        // Human-readable address
        public string Location { get; set; } = default!;

        // ✅ NEW
        public string District { get; set; } = default!;

        public List<PreRegisteredLot> Lots { get; set; } = new();
        public List<MandiOfficial> Officials { get; set; } = new();
        public List<Auction> Auctions { get; set; } = new();
    }
}
