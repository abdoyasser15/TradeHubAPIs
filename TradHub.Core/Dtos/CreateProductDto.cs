using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = default!;

        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile? Image { get; set; } 

        public List<CreateProductAttributeDto>? Attributes { get; set; }
    }
}
