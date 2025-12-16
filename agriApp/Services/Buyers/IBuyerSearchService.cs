using agriApp.Dtos.Buyers;

namespace agriApp.Services.Buyers
{
    public interface IBuyerSearchService
    {
        Task<List<BuyerSearchResultDto>> SearchBuyersAsync(string query);
    }
}
