using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity.Basket
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string LogoUrl { get; set; } = default!;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ICollection<BasketItemOptions> Options { get; set; } = new List<BasketItemOptions>();
    }
}
