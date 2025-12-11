using agriApp.Data;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Services.Crops
{
    public class CropLookupService : ICropLookupService
    {
        private readonly AgriDbContext _db;

        public CropLookupService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task<int?> GetCropIdByNameAsync(string name)
        {
            var crop = await _db.Crops
                .Where(c => c.CropName.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

            return crop?.CropId;
        }

        public async Task<List<string>> GetNamesByIds(List<int> ids)
        {
            return await _db.Crops
                .Where(c => ids.Contains(c.CropId))
                .Select(c => c.CropName)
                .ToListAsync();
        }

        public async Task<Dictionary<int, string>> GetAllCropsAsync()
        {
            return await _db.Crops
                .ToDictionaryAsync(c => c.CropId, c => c.CropName);
        }
    }
}
