using agriApp.Data;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly AgriDbContext _db;

        public RoleService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task<RoleStatusResult> GetRoleStatusAsync(Guid userId)
        {
            var result = new RoleStatusResult
            {
                // IsFarmer = await _db.Set<Farmer>().AnyAsync(x => x.UserId == userId),
                // IsBuyer = await _db.Set<Buyer>().AnyAsync(x => x.UserId == userId),
                // IsSeller = await _db.Set<Seller>().AnyAsync(x => x.UserId == userId),
                // IsMandiOfficial = await _db.Set<MandiOfficial>().AnyAsync(x => x.UserId == userId)
                 IsFarmer = await _db.Farmers.AnyAsync(f => f.UserId == userId),
        IsBuyer = await _db.Buyers.AnyAsync(b => b.UserId == userId),

        // To be implemented later:
        // IsSeller = false,
        IsMandiOfficial = false,

        IsSeller = await _db.Sellers.AnyAsync(s => s.UserId == userId)
// IsMandiOfficial = await _db.MandiOfficials.AnyAsync(m => m.UserId == userId)

            };

            return result;
        }
    }
}
