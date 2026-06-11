using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class ProductOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsRequired { get; set; }
        public bool AllowMultiple { get; set; }

        public List<ProductOptionValueDto> Values { get; set; } = new();

    }
}
