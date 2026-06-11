using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(
       IWebHostEnvironment env,
       IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string?> UploadImageAsync(IFormFile? file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid image type");

            var uploadsFolder = Path.Combine(_env.WebRootPath, folderName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var request = _httpContextAccessor.HttpContext!.Request;

            return $"{request.Scheme}://{request.Host}/{folderName}/{fileName}";
        }
        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            var fullPath = Path.Combine(
                _env.WebRootPath,
                imagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
