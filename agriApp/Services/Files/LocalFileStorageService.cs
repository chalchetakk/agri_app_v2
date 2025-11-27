using Microsoft.AspNetCore.Http;

namespace agriApp.Services.Files
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorageService(IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _basePath = Path.Combine(env.WebRootPath ?? "wwwroot", "uploads");
            _httpContextAccessor = accessor;

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            var folderPath = Path.Combine(_basePath, folder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Build full URL
            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}/uploads/{folder}/{fileName}";
        }

        public Task<bool> DeleteAsync(string fileUrl)
        {
            try
            {
                var fileName = Path.GetFileName(fileUrl);
                var filePath = Directory.GetFiles(_basePath, "*", SearchOption.AllDirectories)
                                        .FirstOrDefault(path => path.EndsWith(fileName));

                if (filePath != null && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }
            }
            catch { }

            return Task.FromResult(false);
        }
    }
}
