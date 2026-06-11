using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Enums;

namespace TradHub.Core.Entity.Orders
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public string BuyerId { get; set; } = default!;

        public double SubTotal { get; set; }
        public double DeliveryFee { get; set; }
        public double Total { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public string PaymentProvider { get; set; } = "Paymob";
        public string? PaymentIntentId { get; set; }
        public string? PaymentTransactionId { get; set; }
        public string? PaymentOrderReference { get; set; }

        public string Address { get; set; } = default!;
        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string CompanyLogoUrl { get; set; } = string.Empty;

        public string? CardLast4Digits { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? RefundedAt { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
