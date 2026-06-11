using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity.Basket
{
    public class BasketItemOptions
    {
        public int BaskItemOptionId { get; set; }
        public int BasketItemId { get; set; }
        public BasketItem BasketItem { get; set; }
        public int ProductOptionValueId { get; set; }
        public ProductOptionValue ProductOptionValue { get; set; } = null!;
        public string OptionName { get; set; } = null!;
        public string ValueName { get; set; } = null!;
        public double ExtraPrice { get; set; }
    }
}
