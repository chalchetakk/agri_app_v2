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

        public async Task<List<MandiDto>> GetMandisAsync()
        {
            return await _db.Mandis
                .Select(m => new MandiDto
                {
                    MandiId = m.MandiId,
                    MandiName = m.MandiName,
                    Location = m.Location
                })
                .ToListAsync();
        }
    }
}
