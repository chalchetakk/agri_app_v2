using Microsoft.AspNetCore.Http;

namespace agriApp.Services.Files
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(IFormFile file, string folder);
        Task<bool> DeleteAsync(string fileUrl);
    }
}
