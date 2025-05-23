using Microsoft.AspNetCore.Http;
using pd311_web_api.DAL.Entities;

namespace pd311_web_api.BLL.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string directoryPath);
        bool DeleteImage(string imagePath);
        void CreateImagesDirectory(string path);
        Task<List<CarImage>> SaveCarImagesAsync(IEnumerable<IFormFile> images, string directoryPath);
    }
}
