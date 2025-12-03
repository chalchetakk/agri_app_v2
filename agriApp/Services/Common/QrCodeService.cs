using QRCoder;

public interface IQrCodeService
{
    Task<string> GenerateLotQrAsync(string text, string folder = "qrcodes");
}

public class QrCodeService : IQrCodeService
{
    private readonly IWebHostEnvironment _env;

    public QrCodeService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> GenerateLotQrAsync(string text, string folder = "qrcodes")
    {
        var qrGen = new QRCodeGenerator();
        var data = qrGen.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

        var qrCode = new PngByteQRCode(data);
        var bytes = qrCode.GetGraphic(20);

        var fileName = $"{Guid.NewGuid()}.png";
        var dir = Path.Combine(_env.WebRootPath, folder);

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var filePath = Path.Combine(dir, fileName);
        await File.WriteAllBytesAsync(filePath, bytes);

        // Return URL (not file system path)
        return $"/{folder}/{fileName}";
    }
}
