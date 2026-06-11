using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class BasketDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; } = default!;
        public string CompanyName { get; set; }
        public Guid CompanyId { get; set; }
        public string LogoUrl { get; set; }
        public List<BasketItemDto> Items { get; set; } = new();
        public double SubTotal => Items.Sum(x => x.Total);
    }
}
