using agriApp.Data;
using agriApp.Dtos.Lots;
using Microsoft.EntityFrameworkCore;

namespace agriApp.Services.Lots
{
    public interface IArrivedLotQueryService
    {
        Task<List<ArrivedLotListItemDto>> GetArrivedLotsForMandiAsync(int mandiId);
        Task<ArrivedLotDetailDto?> GetArrivedLotByIdAsync(int arrivedLotId);
    }
    public class ArrivedLotQueryService : IArrivedLotQueryService
    {
        private readonly AgriDbContext _db;

        public ArrivedLotQueryService(AgriDbContext db)
        {
            _db = db;
        }

        public async Task<List<ArrivedLotListItemDto>> GetArrivedLotsForMandiAsync(int mandiId)
        {
            return await _db.ArrivedLots
                .Include(a => a.Crop)
                .Include(a => a.Mandi)
                .Where(a => a.MandiId == mandiId)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new ArrivedLotListItemDto
                {
                    ArrivedLotId = a.ArrivedLotId,
                    PreLotId = a.PreLotId,
                    CropName = a.Crop!.CropName,
                    MandiName = a.Mandi!.MandiName,
                    Status = a.Status,
                    Quantity = a.Quantity,
                    Grade = a.Grade,
                    LotImageUrl = a.LotImageUrl,
                    QrCodeUrl = a.QrCodeUrl,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ArrivedLotDetailDto?> GetArrivedLotByIdAsync(int arrivedLotId)
        {
            var arrived = await _db.ArrivedLots
                .Include(a => a.Crop)
                .Include(a => a.Mandi)
                .Include(a => a.PreRegisteredLot)
                .FirstOrDefaultAsync(a => a.ArrivedLotId == arrivedLotId);

            if (arrived == null)
                return null;

            return new ArrivedLotDetailDto
            {
                ArrivedLotId = arrived.ArrivedLotId,
                PreLotId = arrived.PreLotId,
                CropId = arrived.CropId,
                CropName = arrived.Crop!.CropName,
                MandiId = arrived.MandiId,
                MandiName = arrived.Mandi!.MandiName,
                Status = arrived.Status,
                Quantity = arrived.Quantity,
                Grade = arrived.Grade,
                LotImageUrl = arrived.LotImageUrl,
                QrCodeUrl = arrived.QrCodeUrl,
                ExpectedArrivalDate = arrived.PreRegisteredLot?.ExpectedArrivalDate,
                SellingAmount = arrived.PreRegisteredLot?.SellingAmount,
                CreatedAt = arrived.CreatedAt
            };
        }
    }
}
