using Microsoft.AspNetCore.Http;

namespace pv311_web_api.BLL.Services.Storage
{
    public interface IStorageService
    {
        Task<string?> UploadFileAsync(IFormFile file, string containerName, string directoryPath);
        Task<string?> UploadImageAsync(IFormFile image, string directoryPath);
        Task DeleteFileAsync(string containerName, string filePath);
        Task DeleteImageAsync(string filePath);
    }
}
