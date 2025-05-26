using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace pv311_web_api.BLL.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string ImagesContainer = "images";

        public StorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task DeleteFileAsync(string containerName, string filePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blob = containerClient.GetBlobClient(filePath);
            await blob.DeleteIfExistsAsync();
        }

        public async Task DeleteImageAsync(string filePath)
        {
            await DeleteFileAsync(ImagesContainer, filePath);
        }

        public async Task<string?> UploadFileAsync(IFormFile file, string containerName, string directoryPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            string fileName = $"{Guid.NewGuid()}.{GetFileExtension(file)}";
            string filePath = Path.Combine(directoryPath, fileName);
            var blob = containerClient.GetBlobClient(filePath);

            using (var stream = file.OpenReadStream())
            {
                var uploadOptions = new BlobUploadOptions();
                uploadOptions.HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };
                var response = await blob.UploadAsync(stream, options: uploadOptions);
                int status = response.GetRawResponse().Status;
                if (status >= 200 && status < 300)
                {
                    return filePath;
                }

                return null;
            }
        }

        public async Task<string?> UploadImageAsync(IFormFile image, string directoryPath)
        {
            var types = image.ContentType.Split('/');
            if (types[0] != "image")
            {
                return null;
            }

            return await UploadFileAsync(image, ImagesContainer, directoryPath);
        }

        private string GetFileExtension(IFormFile file)
        {
            var types = file.ContentType.Split('/');
            if (types.Length == 2)
            {
                return types[1];
            }

            return string.Empty;
        }
    }
}
