using System;
using System.Threading.Tasks;
// using agriApp.DTOs.Buyers;
using agriApp.Controllers;

namespace agriApp.Services.Buyers
{
    public interface IBuyerService
    {
        Task<bool> IsBuyerAsync(Guid userId);
        Task<BuyerResponseDto> RegisterBuyerAsync(Guid userId, BuyerRegisterRequest request);
        Task<BuyerResponseDto?> GetBuyerProfileAsync(Guid userId);
    }
}
