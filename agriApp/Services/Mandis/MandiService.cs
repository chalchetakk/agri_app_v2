using agriApp.Data;
using Microsoft.EntityFrameworkCore;
using agriApp.Controllers;

namespace agriApp.Services.Mandis
{
    public class MandiService : IMandiService
    {
        private readonly AgriDbContext _db;

        public MandiService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task<List<MandiDto>> GetMandisAsync(string? district = null)
        {
            // return await _db.Mandis
            //     .Select(m => new MandiDto
            //     {
            //         MandiId = m.MandiId,
            //         MandiName = m.MandiName,
            //         Location = m.Location
            //     })
            //     .ToListAsync();
            var query = _db.Mandis.AsQueryable();

            if (!string.IsNullOrWhiteSpace(district))
            {
                // query = query.Where(m => m.District == district);
                var normalizedDistrict = district.Trim().ToLower();

query = query.Where(m => m.District.ToLower() == normalizedDistrict);
            }

            return await query
                .OrderBy(m => m.MandiName)
                .Select(m => new MandiDto
                {
                    MandiId = m.MandiId,
                    MandiName = m.MandiName,
                    Location = m.Location,
                    District = m.District
                })
                .ToListAsync();
        }
    }
}
