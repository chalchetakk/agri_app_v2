// using agriApp.Data;
// using Microsoft.EntityFrameworkCore;
// using agriApp.Controllers;

// namespace agriApp.Services.Mandis
// {
//     public class MandiService : IMandiService
//     {
//         private readonly AgriDbContext _db;

//         public MandiService(AgriDbContext db)
//         {
//             _db = db;
//         }

//         public async Task<List<MandiDto>> GetMandisAsync(string? district = null)
//         {
//             // return await _db.Mandis
//             //     .Select(m => new MandiDto
//             //     {
//             //         MandiId = m.MandiId,
//             //         MandiName = m.MandiName,
//             //         Location = m.Location
//             //     })
//             //     .ToListAsync();
//             var query = _db.Mandis.AsQueryable();

//             if (!string.IsNullOrWhiteSpace(district))
//             {
//                 // query = query.Where(m => m.District == district);
//                 var normalizedDistrict = district.Trim().ToLower();

// query = query.Where(m => m.District.ToLower() == normalizedDistrict);
//             }

//             return await query
//                 .OrderBy(m => m.MandiName)
//                 .Select(m => new MandiDto
//                 {
//                     MandiId = m.MandiId,
//                     MandiName = m.MandiName,
//                     Location = m.Location,
//                     District = m.District
//                 })
//                 .ToListAsync();
//         }
//     }
// }


using agriApp.Data;
using agriApp.Controllers;
using Microsoft.EntityFrameworkCore;

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
            var query = _db.Mandis
                           .AsNoTracking()
                           .Where(m => m.IsActive);

            if (!string.IsNullOrWhiteSpace(district))
            {
                var normalizedDistrict = district.Trim().ToLower();;

                // 🔹 STEP 1: Resolve district name → district_id
                var districtEntity = await _db.GeoDistricts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d =>
                        d.DistrictName.ToLower() == normalizedDistrict &&
                        d.IsActive
                    );

                // 🔹 STEP 2: If district not found → return empty list
                if (districtEntity == null)
                {
                    return new List<MandiDto>();
                }

                // 🔹 STEP 3: Filter mandis by district_id
                query = query.Where(m => m.DistrictId == districtEntity.DistrictId);
            }

            // 🔹 STEP 4: Return results (frontend unchanged)
            return await query
                .OrderBy(m => m.MandiName)
                .Select(m => new MandiDto
                {
                    MandiId = m.MandiId,
                    MandiName = m.MandiName,
                    Location = m.Location,
                    District = m.GeoDistrict != null
                        ? m.GeoDistrict.DistrictName
                        : m.District   // fallback for legacy data
                })
                .ToListAsync();
        }
    }
}
