using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class ProductOptions : BaseEntity
    {
        public int ProductOptionId { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public bool AllowMultiple { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public List<ProductOptionValue> ProductOptionValues { get; set; } = new List<ProductOptionValue>();
    }
}
