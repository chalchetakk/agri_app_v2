using System;
using System.Threading.Tasks;
using agriApp.Entities.Stakeholders;
using agriApp.DTOs.Anchors;

namespace agriApp.Services.Anchors
{
    public interface IAnchorService
    {
        Task RegisterAnchorAsync(Guid userId, RegisterAnchorRequest request);
        Task<Anchor?> GetAnchorProfileAsync(Guid userId);
        Task<bool> IsUserAnchorAsync(Guid userId);
        Task UpdateAnchorAsync(Guid userId, UpdateAnchorRequest request);
    }
}
