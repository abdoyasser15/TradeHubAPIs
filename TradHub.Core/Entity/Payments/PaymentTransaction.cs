using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Enums;

namespace TradHub.Core.Entity.Payments
{
    public class PaymentTransaction : BaseEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Provider { get; set; } = "Paymob";
        public string? ProviderTransactionId { get; set; }
        public string? ProviderIntentId { get; set; }
        public string? ProviderOrderReference { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public bool IsPending { get; set; }
        public bool IsSuccess { get; set; }
        public string RawPayload { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
