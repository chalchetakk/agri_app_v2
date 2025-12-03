using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using agriApp.Data;
using agriApp.Entities.Lots;
using agriApp.Services.Files;
using QRCoder;

namespace agriApp.Services.Lots
{
    public class ArrivedLotService : IArrivedLotService
    {
        private readonly AgriDbContext _db;
        private readonly IFileStorageService _fileStorage;

        public ArrivedLotService(AgriDbContext db, IFileStorageService fileStorage)
        {
            _db = db;
            _fileStorage = fileStorage;
        }

        // Generate QR for ArrivedLotId
        private async Task<string> GenerateArrivedQrAsync(int arrivedLotId)
        {
            using var qrGen = new QRCodeGenerator();
            var qrData = qrGen.CreateQrCode(arrivedLotId.ToString(), QRCodeGenerator.ECCLevel.Q);
            var qrPng = new PngByteQRCode(qrData).GetGraphic(20);

            using var ms = new MemoryStream(qrPng);
            var qrFile = new FormFile(ms, 0, ms.Length, "qr", $"{arrivedLotId}.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };

            return await _fileStorage.UploadAsync(qrFile, "arrived-lots/qrcodes");
        }

        // Create ArrivedLot (manual or from preLot)
        public async Task<ArrivedLot> CreateArrivedLotAsync(ArrivedLot lot)
{
    lot.CreatedAt = DateTime.UtcNow;
    lot.UpdatedAt = DateTime.UtcNow;

    // 🔥 FIX: temporary non-null so database doesn’t fail
    lot.QrCodeUrl = "pending";

    _db.ArrivedLots.Add(lot);
    await _db.SaveChangesAsync();  // now DB accepts row because QrCodeUrl has value

    // Generate real QR code
    lot.QrCodeUrl = await GenerateArrivedQrAsync(lot.ArrivedLotId);
    lot.UpdatedAt = DateTime.UtcNow;

    _db.ArrivedLots.Update(lot);
    await _db.SaveChangesAsync();

    return lot;
}


        // Edit ArrivedLot anytime before sold/unsold
        public async Task<ArrivedLot?> EditArrivedLotAsync(int arrivedLotId, Action<ArrivedLot> applyChanges)
        {
            var lot = await _db.ArrivedLots.FirstOrDefaultAsync(x => x.ArrivedLotId == arrivedLotId);
            if (lot == null) return null;

            // Cannot edit after auction result
            var auction = await _db.LiveAuctionLots
                .FirstOrDefaultAsync(x => x.ArrivedLotId == arrivedLotId);

            if (auction != null && (auction.AuctionStatus == "sold" || auction.AuctionStatus == "unsold"))
                throw new Exception("Cannot edit lot after sold/unsold.");

            applyChanges(lot);

            lot.UpdatedAt = DateTime.UtcNow;
            _db.ArrivedLots.Update(lot);

            await _db.SaveChangesAsync();
            return lot;
        }

        // Status update: arrived → verified → readyForAuction
        public async Task<ArrivedLot?> UpdateStatusAsync(int arrivedLotId, string newStatus)
        {
            var lot = await _db.ArrivedLots.FirstOrDefaultAsync(x => x.ArrivedLotId == arrivedLotId);
            if (lot == null) return null;

            if (lot.Status == "arrived" && newStatus == "verified") { }
            else if (lot.Status == "verified" && newStatus == "readyForAuction") { }
            else
                throw new Exception("Invalid status transition.");

            lot.Status = newStatus;
            lot.UpdatedAt = DateTime.UtcNow;

            _db.ArrivedLots.Update(lot);
            await _db.SaveChangesAsync();

            return lot;
        }
    }
}
 