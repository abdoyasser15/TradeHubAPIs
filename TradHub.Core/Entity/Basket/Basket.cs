using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity.Basket
{
    public class Basket
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;
        public List<BasketItem> Items { get; set; } = new List<BasketItem>(); 
    }
}
