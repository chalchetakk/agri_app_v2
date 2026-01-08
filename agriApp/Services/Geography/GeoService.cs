using agriApp.Data;
using agriApp.Dtos.Geography;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Services.Geography
{
    public class GeoService : IGeoService
    {
        private readonly AgriDbContext _db;

        public GeoService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task<List<StateDto>> GetStatesAsync()
        {
            return await _db.GeoStates
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderBy(s => s.StateName)
                .Select(s => new StateDto
                {
                    StateId = s.StateId,
                    StateName = s.StateName
                })
                .ToListAsync();
        }

        public async Task<List<DistrictDto>> GetDistrictsByStateAsync(string stateName)
        {
            var normalized = stateName.Trim().ToLower();

            var state = await _db.GeoStates
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.StateName.ToLower() == normalized &&
                    s.IsActive
                );

            if (state == null)
                return new List<DistrictDto>();

            return await _db.GeoDistricts
                .AsNoTracking()
                .Where(d =>
                    d.StateId == state.StateId &&
                    d.IsActive
                )
                .OrderBy(d => d.DistrictName)
                .Select(d => new DistrictDto
                {
                    DistrictId = d.DistrictId,
                    DistrictName = d.DistrictName
                })
                .ToListAsync();
        }
    }
}
