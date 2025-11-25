using System.Collections.Generic;
using System.Threading.Tasks;
using agriApp.Controllers;
namespace agriApp.Services.Crops
{
    public interface ICropService
    {
        Task<List<CropDto>> GetAllCropsAsync();
    }
}
