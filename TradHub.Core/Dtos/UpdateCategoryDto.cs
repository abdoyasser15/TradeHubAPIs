using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class UpdateCategoryDto
    {
        [FromForm(Name = "name")]
        public string Name { get; set; } = default!;
        [FromForm(Name = "image")]
        public IFormFile? Image { get; set; }
    }
}
