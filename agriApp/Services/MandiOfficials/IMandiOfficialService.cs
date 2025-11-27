using System;
using System.Threading.Tasks;
using agriApp.Controllers;

namespace agriApp.Services.MandiOfficials
{
    public interface IMandiOfficialService
    {
        Task RegisterOfficialAsync(Guid userId, MandiOfficialRegisterRequest request);
        Task<bool> OfficialExistsAsync(Guid userId);
        Task<MandiOfficialProfileDto?> GetOfficialProfileAsync(Guid userId);
        Task<List<OfficialRoleDto>> GetRolesAsync();

    }
}
