using agriApp.DTOs.Anchors;

namespace agriApp.Services.BulkImport
{
    public interface IBulkFarmerValidationService
    {
        Task ValidateGroupedRowsAsync(List<BulkFarmerGroupedDto> grouped);
    }
}
