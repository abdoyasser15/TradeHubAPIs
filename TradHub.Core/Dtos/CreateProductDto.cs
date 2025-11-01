using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Guid CompanyId { get; set; }
        public int CategoryId { get; set; }

        public List<CreateProductAttributeDto>? Attributes { get; set; }
    }
}
