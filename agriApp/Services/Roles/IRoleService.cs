using System;
using System.Threading.Tasks;

namespace agriApp.Services.Roles
{
    public interface IRoleService
    {
        Task<RoleStatusResult> GetRoleStatusAsync(Guid userId);
    }

    public class RoleStatusResult
    {
        public bool IsFarmer { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsSeller { get; set; }
        public bool IsMandiOfficial { get; set; }
        public bool IsAnchor {get; set;}
    }
}
