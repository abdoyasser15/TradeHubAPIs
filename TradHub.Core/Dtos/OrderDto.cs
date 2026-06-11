using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Enums;

namespace TradHub.Core.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public double SubTotal { get; set; }
        public double DeliveryFee { get; set; }
        public double Total { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLogo { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string? MaskedCardNumber { get; set; }
    }
}
