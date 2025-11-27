using agriApp.Controllers;

namespace agriApp.Services.Mandis
{
    public interface IMandiService
    {
        Task<List<MandiDto>> GetMandisAsync();
    }
}
