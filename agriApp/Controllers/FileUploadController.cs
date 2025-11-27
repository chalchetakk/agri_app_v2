using agriApp.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace agriApp.Controllers
{
    [ApiController]
    [Route("files")]
    [Authorize]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileStorageService _fileStorage;

        public FileUploadController(IFileStorageService fileStorage)
        {
            _fileStorage = fileStorage;
        }

        /// <summary>
        /// Upload a file to a specific folder (example: farmers/profile-photos)
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest(new { message = "File is required." });

            if (string.IsNullOrWhiteSpace(request.Folder))
                return BadRequest(new { message = "Folder path is required." });

            var url = await _fileStorage.UploadAsync(request.File, request.Folder);

            return Ok(new { url });
        }

        /// <summary>
        /// Delete a previously uploaded file using its URL
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] FileDeleteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FileUrl))
                return BadRequest(new { message = "FileUrl is required." });

            var success = await _fileStorage.DeleteAsync(request.FileUrl);

            return Ok(new { deleted = success });
        }
    }

    // DTOs
    public class FileUploadRequest
    {
        public IFormFile File { get; set; } = default!;
        public string Folder { get; set; } = default!;
    }

    public class FileDeleteRequest
    {
        public string FileUrl { get; set; } = default!;
    }
}
