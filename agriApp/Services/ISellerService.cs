using System;
using System.Threading.Tasks;
using agriApp.Controllers; 

namespace agriApp.Services.Sellers
{
    public interface ISellerService
    {
        Task RegisterSellerAsync(Guid userId, SellerRegisterRequest request);
        Task<bool> IsSellerExistsAsync(Guid userId);
    }
}
