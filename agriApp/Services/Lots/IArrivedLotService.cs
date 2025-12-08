using agriApp.Entities.Lots;

namespace agriApp.Services.Lots
{
    public interface IArrivedLotService
    {
        Task<ArrivedLot> CreateArrivedLotAsync(ArrivedLot lot);
        Task<ArrivedLot?> EditArrivedLotAsync(int arrivedLotId, Action<ArrivedLot> applyChanges);

        // UPDATED: requires auctionId for readyForAuction
        Task<ArrivedLot?> UpdateStatusAsync(int arrivedLotId, string newStatus, Guid? auctionId = null);
    }
}
