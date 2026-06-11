using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Service_Contract
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
        void DeleteImage(string imagePath);
    }
}
