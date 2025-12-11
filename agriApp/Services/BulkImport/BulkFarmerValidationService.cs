using agriApp.Data;
using agriApp.DTOs.Anchors;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Services.BulkImport
{
    public class BulkFarmerValidationService : IBulkFarmerValidationService
    {
        private readonly AgriDbContext _db;

        public BulkFarmerValidationService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task ValidateGroupedRowsAsync(List<BulkFarmerGroupedDto> grouped)
        {
            foreach (var g in grouped)
            {
                // 1. Validate consistent farmer name
                if (g.GroupRows.Select(x => x.FarmerName).Distinct().Count() > 1)
                    throw new Exception($"Farmer '{g.Mobile}' has mismatched names across rows.");

                // 2. Check if farmer already exists in DB
                bool exists = await _db.Farmers.AnyAsync(f => f.User!.MobileNumber == g.Mobile);
                if (exists)
                    throw new Exception($"Farmer with mobile {g.Mobile} already exists in system.");

                // 3. Validate crop names for existence later in crop lookup
                foreach (var crop in g.AllInterestedCrops)
                {
                    if (string.IsNullOrWhiteSpace(crop))
                        throw new Exception($"A crop name cannot be empty in farmer {g.Mobile}.");
                }

                // 4. Validate farm details
                if (!g.Farms.Any())
                    throw new Exception($"Farmer {g.Mobile} must have at least one farm.");

                foreach (var farm in g.Farms)
                {
                    if (string.IsNullOrWhiteSpace(farm.FarmLocation))
                        throw new Exception($"Farmer {g.Mobile}: FarmLocation is required.");

                    if (string.IsNullOrWhiteSpace(farm.PrimaryCrop))
                        throw new Exception($"Farmer {g.Mobile}: PrimaryCrop is required.");

                    if (farm.FarmSize <= 0)
                        throw new Exception($"Farmer {g.Mobile}: FarmSize must be > 0.");
                }
            }
        }
    }
}
