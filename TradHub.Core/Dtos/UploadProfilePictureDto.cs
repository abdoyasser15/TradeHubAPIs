using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class UploadProfilePictureDto
    {
        public IFormFile Image { get; set; } = null!;
    }
}
