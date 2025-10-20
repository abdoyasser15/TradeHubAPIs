using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class ProductAttribute : BaseEntity
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int CategoryAttributeId { get; set; }
        public CategoryAttribute CategoryAttribute { get; set; } = null!;
    }
}
