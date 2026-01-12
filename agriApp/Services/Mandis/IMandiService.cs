using agriApp.Controllers;

namespace agriApp.Services.Mandis
{
    public interface IMandiService
    {
        Task<List<MandiDto>> GetMandisAsync(string? state = null,string? district = null, string? category   = null);
    }
}
