using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace pd311_web_api.BLL.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string ImagesContainer = "images";

        public StorageService(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("StorageService");
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task DeleteFileAsync(string containerName, string filePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var client = containerClient.GetBlobClient(filePath);
            await client.DeleteIfExistsAsync();
        }

        public async Task DeleteImageAsync(string filePath)
        {
            await DeleteFileAsync(ImagesContainer, filePath);
        }

        public async Task<string?> UploadFileAsync(IFormFile file, string containerName, string path)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            string fileName = $"{Guid.NewGuid()}.{GetFileExtension(file.ContentType)}";
            string filePath = Path.Combine(path, fileName);
            var client = containerClient.GetBlobClient(filePath);
            using(var stream = file.OpenReadStream())
            {
                var response = await client.UploadAsync(file.OpenReadStream());
                int status = response.GetRawResponse().Status;
                if (status >= 200 && status < 300)
                {
                    await client.SetHttpHeadersAsync(httpHeaders: new BlobHttpHeaders { ContentType = file.ContentType });
                    return filePath;
                }
                return null;
            }
        }

        public async Task<string?> UploadImageAsync(IFormFile image, string path)
        {
            var types = image.ContentType.Split("/");
            if (types[0] != "image")
            {
                return null;
            }
            return await UploadFileAsync(image, ImagesContainer, path);
        }

        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string path)
        {
            List<string> names = new List<string>();
            foreach (var image in images) 
            {
                var name = await UploadImageAsync(image, path);
                if (name != null)
                {
                    names.Add(name);
                }
            }
            return names;
        }

        private string GetFileExtension(string contentType)
        {
            var types = contentType.Split('/');
            return types[1];
        }
    }
}
