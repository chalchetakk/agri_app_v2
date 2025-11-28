using agriApp.Dtos.Lots;

namespace agriApp.Services.Lots
{
    public interface ILotService
    {
        Task<string> GeneratePreLotIdAsync();

        Task<LotListItemDto> RegisterLotAsync(Guid userId, LotRegisterRequest request, bool isFarmer);

        Task<List<LotListItemDto>> GetMyLotsAsync(Guid userId, bool isFarmer);

        Task<LotDetailDto?> GetLotByIdAsync(string preLotId, Guid userId, bool isFarmer);
        Task<LotDetailDto?> EditLotAsync(string preLotId, Guid userId, bool isFarmer, LotEditRequest request);
 // NEW:
    Task<bool> DeleteLotAsync(string preLotId, Guid userId, bool isFarmer);
    }
}
