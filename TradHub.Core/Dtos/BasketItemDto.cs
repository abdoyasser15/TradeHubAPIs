using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total => Price * Quantity;
        public List<BasketItemOptionsDto> Options { get; set; } = new();
    }
}
