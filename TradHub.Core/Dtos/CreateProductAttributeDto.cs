using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CreateProductAttributeDto
    {
        public int CategoryAttributeId { get; set; }
        public string? Value { get; set; }
    }
}
