using System.Collections.Generic;
using agriApp.Entities.Lots;
using agriApp.Entities.Auctions;
using agriApp.Entities.Stakeholders;
using agriApp.Entities.Geography;
using agriApp.Entities.Lookups;


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


        // 🔹 NEW – Business identifiers
        public string? MandiCode { get; set; }

        // 🔹 NEW – Lookup
        public long? MandiCategoryId { get; set; }
        public MandiCategory? MandiCategory { get; set; } = default!;

        // 🔹 NEW – Geography (authoritative)
        public long? StateId { get; set; }
        public GeoState? State { get; set; }

        public long? DistrictId { get; set; }
        public GeoDistrict? GeoDistrict { get; set; }

        public long? TalukaId { get; set; }
        public GeoTaluka? Taluka { get; set; }

        // 🔹 NEW – Address
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }

        // 🔹 NEW – Location metadata
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // 🔹 NEW – Operational info
        public string? WorkingDays { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }

        // 🔹 NEW – Infrastructure
        public decimal? TotalStorageCapacityMt { get; set; }
        public bool ColdStorageAvailable { get; set; }
        public decimal? ColdStorageCapacityMt { get; set; }
        public bool GradingSortingAvailable { get; set; }
        public string? WeighingType { get; set; }

        // 🔹 NEW – Contact
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? Website { get; set; }

        // 🔹 NEW – Lifecycle
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public List<PreRegisteredLot> Lots { get; set; } = new();
        public List<MandiOfficial> Officials { get; set; } = new();
        public List<Auction> Auctions { get; set; } = new();
    }
}
