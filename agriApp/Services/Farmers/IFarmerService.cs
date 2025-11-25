using System;
using System.Threading.Tasks;
using agriApp.Controllers; // if DTO is in controller for now

namespace agriApp.Services.Farmers
{
    public interface IFarmerService
    {
        Task RegisterFarmerAsync(Guid userId, FarmerRegisterRequest request);
    }
}
