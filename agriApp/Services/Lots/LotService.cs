using System.Security.Cryptography;
using agriApp.Data;
using agriApp.Dtos.Lots;
using agriApp.Entities.Lots;
using agriApp.Services.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace agriApp.Services.Lots
{
    public class LotService : ILotService
    {
        private readonly AgriDbContext _db;
        private readonly IFileStorageService _fileStorage;
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public LotService(AgriDbContext db, IFileStorageService fileStorage)
        {
            _db = db;
            _fileStorage = fileStorage;
        }

        // -----------------------------------------------------
        // Generate unique 10-digit ID
        // -----------------------------------------------------
        public async Task<string> GeneratePreLotIdAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                var id = Generate10Digit();
                if (!await _db.PreRegisteredLots.AnyAsync(l => l.PreLotId == id))
                    return id;
            }

            return DateTime.UtcNow.Ticks.ToString().Substring(0, 10);
        }

        private string Generate10Digit()
        {
            var b = new byte[8];
            _rng.GetBytes(b);
            ulong num = BitConverter.ToUInt64(b, 0);

            ulong min = 1000000000UL;
            ulong max = 9999999999UL;

            return (min + (num % (max - min + 1))).ToString();
        }

        // -----------------------------------------------------
        // Register Lot
        // -----------------------------------------------------
        public async Task<LotListItemDto> RegisterLotAsync(Guid userId, LotRegisterRequest request, bool isFarmer)
        {
            if (request.ExpectedArrivalDate.Date < DateTime.UtcNow.Date)
                throw new Exception("Expected arrival date cannot be in the past.");

            var crop = await _db.Crops.FirstOrDefaultAsync(c => c.CropId == request.CropId)
                ?? throw new Exception("Invalid crop.");

            var mandi = await _db.Mandis.FirstOrDefaultAsync(m => m.MandiId == request.MandiId)
                ?? throw new Exception("Invalid mandi.");

            Guid? farmerId = null;
            Guid? sellerId = null;

            if (isFarmer)
            {
                var farmer = await _db.Farmers.FirstOrDefaultAsync(f => f.UserId == userId)
                    ?? throw new Exception("Not registered as farmer.");
                farmerId = farmer.FarmerId;
            }
            else
            {
                var seller = await _db.Sellers.FirstOrDefaultAsync(s => s.UserId == userId)
                    ?? throw new Exception("Not registered as seller.");
                sellerId = seller.SellerId;
            }

            string preLotId = await GeneratePreLotIdAsync();

            string lotImageUrl = "";
            if (request.LotImage != null)
                lotImageUrl = await _fileStorage.UploadAsync(request.LotImage, "lots/images");

            var entity = new PreRegisteredLot
            {
                PreLotId = preLotId,
                FarmerId = farmerId,
                SellerId = sellerId,
                CropId = request.CropId,
                MandiId = request.MandiId,

                Quantity = request.Quantity,
                Grade = request.Grade,
                ExpectedArrivalDate = request.ExpectedArrivalDate,

                SellingAmount = request.SellingAmount,

                LotImageUrl = lotImageUrl,
                Status = "preRegistered",

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.PreRegisteredLots.Add(entity);
            await _db.SaveChangesAsync();

            // QR Code generation
            var qrBytes = GenerateQr(preLotId);
            using var ms = new MemoryStream(qrBytes);
            var qrFile = new FormFile(ms, 0, ms.Length, "qr", $"{preLotId}.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };

            entity.QrCodeUrl = await _fileStorage.UploadAsync(qrFile, "lots/qrcodes");
            entity.UpdatedAt = DateTime.UtcNow;

            _db.PreRegisteredLots.Update(entity);
            await _db.SaveChangesAsync();

            return new LotListItemDto
            {
                PreLotId = entity.PreLotId,
                Status = entity.Status,

                LotImageUrl = entity.LotImageUrl,
                QrCodeUrl = entity.QrCodeUrl,

                CropName = crop.CropName,
                MandiName = mandi.MandiName,

                ExpectedArrivalDate = entity.ExpectedArrivalDate,
                Grade = entity.Grade,
                Quantity = entity.Quantity,
                SellingAmount = entity.SellingAmount
            };
        }

        // -----------------------------------------------------
        // Fetch My Lots
        // -----------------------------------------------------
        public async Task<List<LotListItemDto>> GetMyLotsAsync(Guid userId, bool isFarmer)
        {
            Guid ownerId = isFarmer
                ? (await _db.Farmers.FirstAsync(f => f.UserId == userId)).FarmerId
                : (await _db.Sellers.FirstAsync(s => s.UserId == userId)).SellerId;

            var lots = await _db.PreRegisteredLots
                .Include(l => l.Crop)
                .Include(l => l.Mandi)
                .Where(l => (isFarmer ? l.FarmerId : l.SellerId) == ownerId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return lots.Select(l => new LotListItemDto
            {
                PreLotId = l.PreLotId,
                Status = l.Status,
                LotImageUrl = l.LotImageUrl,
                QrCodeUrl = l.QrCodeUrl,
                CropName = l.Crop?.CropName ?? "",
                MandiName = l.Mandi?.MandiName ?? "",
                ExpectedArrivalDate = l.ExpectedArrivalDate,
                Grade = l.Grade,
                Quantity = l.Quantity,
                SellingAmount = l.SellingAmount
            }).ToList();
        }

        // -----------------------------------------------------
        // Lot Details
        // -----------------------------------------------------
        public async Task<LotDetailDto?> GetLotByIdAsync(string preLotId, Guid userId, bool isFarmer)
        {
            var lot = await _db.PreRegisteredLots
                .Include(l => l.Crop)
                .Include(l => l.Mandi)
                .FirstOrDefaultAsync(l => l.PreLotId == preLotId);

            if (lot == null) return null;

            Guid ownerId = isFarmer
                ? (await _db.Farmers.FirstAsync(f => f.UserId == userId)).FarmerId
                : (await _db.Sellers.FirstAsync(s => s.UserId == userId)).SellerId;

            if ((isFarmer ? lot.FarmerId : lot.SellerId) != ownerId)
                return null; // Not owner → unauthorized

            return new LotDetailDto
            {
                PreLotId = lot.PreLotId,
                Status = lot.Status,

                CropId = lot.CropId,
                CropName = lot.Crop?.CropName ?? "",

                MandiId = lot.MandiId,
                MandiName = lot.Mandi?.MandiName ?? "",
                MandiLocation = lot.Mandi?.Location ?? "",

                ExpectedArrivalDate = lot.ExpectedArrivalDate,
                Grade = lot.Grade,
                Quantity = lot.Quantity,
                SellingAmount = lot.SellingAmount,

                LotImageUrl = lot.LotImageUrl,
                QrCodeUrl = lot.QrCodeUrl,

                CreatedAt = lot.CreatedAt,
                UpdatedAt = lot.UpdatedAt
            };
        }

        // -----------------------------------------------------
        // QR Generation
        // -----------------------------------------------------
        private byte[] GenerateQr(string text)
        {
            using var gen = new QRCodeGenerator();
            using var data = gen.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            return new PngByteQRCode(data).GetGraphic(20);
        }

        public async Task<LotDetailDto?> EditLotAsync(string preLotId, Guid userId, bool isFarmer, LotEditRequest request)
{
    var lot = await _db.PreRegisteredLots
        .Include(l => l.Crop)
        .Include(l => l.Mandi)
        .FirstOrDefaultAsync(l => l.PreLotId == preLotId);

    if (lot == null)
        return null;

    // -------------------------------
    // VERIFY OWNERSHIP
    // -------------------------------
    if (isFarmer)
    {
        var farmer = await _db.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
        if (farmer == null || lot.FarmerId != farmer.FarmerId)
            throw new Exception("You are not authorized to edit this lot.");
    }
    else
    {
        var seller = await _db.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);
        if (seller == null || lot.SellerId != seller.SellerId)
            throw new Exception("You are not authorized to edit this lot.");
    }

    // -------------------------------
    // VALIDATION
    // -------------------------------
    if (request.Quantity <= 0)
        throw new Exception("Quantity must be greater than zero.");

    if (request.ExpectedArrivalDate.Date < DateTime.UtcNow.Date)
        throw new Exception("Expected arrival date cannot be a past date.");

    var crop = await _db.Crops.FirstOrDefaultAsync(c => c.CropId == request.CropId);
    if (crop == null) throw new Exception("Invalid crop.");

    var mandi = await _db.Mandis.FirstOrDefaultAsync(m => m.MandiId == request.MandiId);
    if (mandi == null) throw new Exception("Invalid mandi.");

    // -------------------------------
    // HANDLE OPTIONAL IMAGE UPDATE
    // -------------------------------
    if (request.LotImage != null && request.LotImage.Length > 0)
    {
        var newImageUrl = await _fileStorage.UploadAsync(request.LotImage, "lots/images");
        lot.LotImageUrl = newImageUrl;
    }

    // -------------------------------
    // UPDATE EDITABLE FIELDS ONLY
    // -------------------------------
    lot.CropId = request.CropId;
    lot.MandiId = request.MandiId;
    lot.Quantity = request.Quantity;
    lot.Grade = request.Grade;
    lot.SellingAmount = request.SellingAmount;
    lot.ExpectedArrivalDate = request.ExpectedArrivalDate;

    lot.UpdatedAt = DateTime.UtcNow;

    _db.PreRegisteredLots.Update(lot);
    await _db.SaveChangesAsync();

    // -------------------------------
    // RETURN UPDATED DTO
    // -------------------------------
    return new LotDetailDto
    {
        PreLotId = lot.PreLotId,
        Status = lot.Status,
        LotImageUrl = lot.LotImageUrl,
        QrCodeUrl = lot.QrCodeUrl,

        CropId = lot.CropId,
        CropName = crop.CropName,

        MandiId = lot.MandiId,
        MandiName = mandi.MandiName,
        MandiLocation = mandi.Location,

        Quantity = lot.Quantity,
        Grade = lot.Grade,
        SellingAmount = lot.SellingAmount,
        ExpectedArrivalDate = lot.ExpectedArrivalDate,

        CreatedAt = lot.CreatedAt,
        UpdatedAt = lot.UpdatedAt,

      
    };
}

public async Task<bool> DeleteLotAsync(string preLotId, Guid userId, bool isFarmer)
{
    if (string.IsNullOrWhiteSpace(preLotId))
        throw new ArgumentException("Invalid lot ID.");

    // Load lot with all required info
    var lot = await _db.PreRegisteredLots
        .Include(l => l.Crop)
        .Include(l => l.Mandi)
        .FirstOrDefaultAsync(l => l.PreLotId == preLotId);

    if (lot == null)
        return false; // Not found

    // Check owner
    if (isFarmer)
    {
        var farmer = await _db.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
        if (farmer == null || lot.FarmerId != farmer.FarmerId)
            return false; // Not owner
    }
    else
    {
        var seller = await _db.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);
        if (seller == null || lot.SellerId != seller.SellerId)
            return false; // Not owner
    }

    // Only preRegistered lots can be deleted
    if (!string.Equals(lot.Status, "preRegistered", StringComparison.OrdinalIgnoreCase))
        throw new Exception("Cannot delete lot after it has progressed beyond 'preRegistered'.");

    // Delete lot image
    if (!string.IsNullOrWhiteSpace(lot.LotImageUrl))
        await _fileStorage.DeleteAsync(lot.LotImageUrl);

    // Delete QR code
    if (!string.IsNullOrWhiteSpace(lot.QrCodeUrl))
        await _fileStorage.DeleteAsync(lot.QrCodeUrl);

    // Remove from DB
    _db.PreRegisteredLots.Remove(lot);
    await _db.SaveChangesAsync();

    return true;
}


    }
}
