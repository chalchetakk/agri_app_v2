using agriApp.Dtos.Geography;

namespace agriApp.Services.Geography
{
    public interface IGeoService
    {
        Task<List<StateDto>> GetStatesAsync();
        Task<List<DistrictDto>> GetDistrictsByStateAsync(string stateName);
    }
}
