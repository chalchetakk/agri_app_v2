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
            };

            return result;
        }
    }
}
