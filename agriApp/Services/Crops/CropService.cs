using agriApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using agriApp.Controllers;

namespace agriApp.Services.Crops
{
    public class CropService : ICropService
    {
        private readonly AgriDbContext _db;

        public CropService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task<List<CropDto>> GetAllCropsAsync()
        {
            var crops = await _db.Crops.OrderBy(x => x.CropName).ToListAsync();

            return crops.Select(c => new CropDto
            {
                CropId = c.CropId,
                CropName = c.CropName,
                Grade = c.Grade
            }).ToList();
        }
    }
}
