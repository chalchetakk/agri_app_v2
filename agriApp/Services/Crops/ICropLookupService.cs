namespace agriApp.Services.Crops
{
    public interface ICropLookupService
    {
        Task<int?> GetCropIdByNameAsync(string name);
        Task<List<string>> GetNamesByIds(List<int> ids);
        Task<Dictionary<int, string>> GetAllCropsAsync();
    }
}
