using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class UpdateProductImageDto
    {
        [Required]
        public IFormFile Image { get; set; } = default!;
    }
}
