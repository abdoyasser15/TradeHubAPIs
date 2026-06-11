using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity
{
    public class ProductOptionValue : BaseEntity
    {
        public int ProductOptionValueId { get; set; }
        public string Name { get; set; }
        public double ExtraPrice { get; set; }
        public int ProductOptionId { get; set; }
        public ProductOptions ProductOptions { get; set; }
    }
}
