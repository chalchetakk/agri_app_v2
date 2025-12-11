using agriApp.Data;
using agriApp.Entities.Stakeholders;
using Microsoft.EntityFrameworkCore;
using agriApp.Controllers;
using agriApp.DTOs.MandiOfficials;

namespace agriApp.Services.MandiOfficials
{
    public class MandiOfficialService : IMandiOfficialService
    {
        private readonly AgriDbContext _db;

        public MandiOfficialService(AgriDbContext db)
        {
            _db = db;
        }

        // -----------------------------------------------------
        // REGISTER
        // -----------------------------------------------------
        public async Task RegisterOfficialAsync(Guid userId, MandiOfficialRegisterRequest request)
        {
            // Prevent duplicates
            if (await _db.MandiOfficials.AnyAsync(x => x.UserId == userId))
                throw new Exception("Already registered as Mandi Official.");

            // Validate mandiId exists
            var mandiExists = await _db.Mandis.AnyAsync(m => m.MandiId == request.MandiId);
            if (!mandiExists)
                throw new Exception("Invalid Mandi.");

            // Validate officialRoleId exists
            var roleExists = await _db.OfficialRoles.AnyAsync(r => r.OfficialRoleId == request.OfficialRoleId);
            if (!roleExists)
                throw new Exception("Invalid Official Role.");

            var official = new MandiOfficial
            {
                OfficialName = request.OfficialName,
                EmployeeId = request.EmployeeId,
                Email = request.Email,
                MandiId = request.MandiId,
                OfficialRoleId = request.OfficialRoleId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.MandiOfficials.Add(official);
            await _db.SaveChangesAsync();
        }

        // -----------------------------------------------------
        // CHECK IF EXISTS
        // -----------------------------------------------------
        public async Task<bool> OfficialExistsAsync(Guid userId)
        {
            return await _db.MandiOfficials.AnyAsync(m => m.UserId == userId);
        }

        // -----------------------------------------------------
        // GET PROFILE
        // -----------------------------------------------------
        public async Task<MandiOfficialProfileDto?> GetOfficialProfileAsync(Guid userId)
        {
            var official = await _db.MandiOfficials
                .Include(m => m.Mandi)
                .Include(m => m.Role)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (official == null)
                return null;

            return new MandiOfficialProfileDto
            {
                OfficialId = official.OfficialId,
                OfficialName = official.OfficialName,
                EmployeeId = official.EmployeeId,
                Email = official.Email,

                MandiId = official.MandiId,
                MandiName = official.Mandi?.MandiName,
                MandiLocation = official.Mandi?.Location,

                OfficialRoleId = official.OfficialRoleId,
                OfficialRoleName = official.Role?.OfficialRoleName,

                CreatedAt = official.CreatedAt,
                UpdatedAt = official.UpdatedAt
            };
        }
        
        public async Task<List<OfficialRoleDto>> GetRolesAsync()
{
    return await _db.OfficialRoles
        .Select(r => new OfficialRoleDto
        {
            OfficialRoleId = r.OfficialRoleId,
            OfficialRoleName = r.OfficialRoleName,
            RoleCode = r.RoleCode
        })
        .ToListAsync();
}
public async Task<List<MandiOfficerListDto>> GetOfficersByMandiIdAsync(int mandiId)
{
    return await _db.MandiOfficials
        .Where(x => x.MandiId == mandiId)
        .Include(x => x.Role)
        .Select(x => new MandiOfficerListDto
        {
            OfficialId = x.OfficialId,
            OfficialName = x.OfficialName,
            Email = x.Email,
            EmployeeId = x.EmployeeId,
            RoleCode = x.Role.RoleCode
        })
        .ToListAsync();
}

    }
}
