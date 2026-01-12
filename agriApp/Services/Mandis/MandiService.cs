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


// using agriApp.Data;
// using agriApp.Controllers;
// using Microsoft.EntityFrameworkCore;

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
//             var query = _db.Mandis
//                            .AsNoTracking()
//                            .Where(m => m.IsActive);

//             if (!string.IsNullOrWhiteSpace(district))
//             {
//                 var normalizedDistrict = district.Trim().ToLower();;

//                 // 🔹 STEP 1: Resolve district name → district_id
//                 var districtEntity = await _db.GeoDistricts
//                     .AsNoTracking()
//                     .FirstOrDefaultAsync(d =>
//                         d.DistrictName.ToLower() == normalizedDistrict &&
//                         d.IsActive
//                     );

//                 // 🔹 STEP 2: If district not found → return empty list
//                 if (districtEntity == null)
//                 {
//                     return new List<MandiDto>();
//                 }

//                 // 🔹 STEP 3: Filter mandis by district_id
//                 query = query.Where(m => m.DistrictId == districtEntity.DistrictId);
//             }

//             // 🔹 STEP 4: Return results (frontend unchanged)
//             return await query
//                 .OrderBy(m => m.MandiName)
//                 .Select(m => new MandiDto
//                 {
//                     MandiId = m.MandiId,
//                     MandiName = m.MandiName,
//                     Location = m.Location,
//                     District = m.GeoDistrict != null
//                         ? m.GeoDistrict.DistrictName
//                         : m.District   // fallback for legacy data
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

        public async Task<List<MandiDto>> GetMandisAsync(
    string? state = null,
    string? district = null,
    string? category = null)
{
    var query = _db.Mandis
        .AsNoTracking()
        .Where(m => m.IsActive);

    long? stateId = null;
    long? districtId = null;
    long? categoryId = null;

    // 🔹 Resolve state
    if (!string.IsNullOrWhiteSpace(state))
    {
        var normalizedState = state.Trim().ToLower();

        var stateEntity = await _db.GeoStates
            .AsNoTracking()
            .FirstOrDefaultAsync(s =>
                s.StateName.ToLower() == normalizedState &&
                s.IsActive
            );

        if (stateEntity == null)
            return new List<MandiDto>();

        stateId = stateEntity.StateId;
        query = query.Where(m => m.StateId == stateId);
    }

    // 🔹 Resolve district (scoped to state if provided)
    if (!string.IsNullOrWhiteSpace(district))
    {
        var normalizedDistrict = district.Trim().ToLower();

        var districtQuery = _db.GeoDistricts
            .AsNoTracking()
            .Where(d =>
                d.DistrictName.ToLower() == normalizedDistrict &&
                d.IsActive
            );

        if (stateId.HasValue)
            districtQuery = districtQuery.Where(d => d.StateId == stateId);

        var districtEntity = await districtQuery.FirstOrDefaultAsync();

        if (districtEntity == null)
            return new List<MandiDto>();

        districtId = districtEntity.DistrictId;
        query = query.Where(m => m.DistrictId == districtId);
    }

    // 🔹 Resolve mandi category
    if (!string.IsNullOrWhiteSpace(category))
    {
        var normalizedCategory = category.Trim().ToLower();

        var categoryEntity = await _db.MandiCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c =>
                c.CategoryName.ToLower() == normalizedCategory &&
                c.IsActive
            );

        if (categoryEntity == null)
            return new List<MandiDto>();

        categoryId = categoryEntity.MandiCategoryId;
        query = query.Where(m => m.MandiCategoryId == categoryId);
    }

    return await query
        .OrderBy(m => m.MandiName)
        .Select(m => new MandiDto
        {
            MandiId = m.MandiId,
            MandiName = m.MandiName,
            Location = m.Location,
            District = m.GeoDistrict != null
                ? m.GeoDistrict.DistrictName
                : m.District
        })
        .ToListAsync();
}

    }
}
