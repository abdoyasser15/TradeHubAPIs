using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CreatePaymentSessionResponse
    {
        public int OrderId { get; set; }
        public string ClientSecret { get; set; } = default!;
        public string PaymentIntentId { get; set; } = default!;
        public string PublicKey { get; set; } = default!;
        public string PaymentUrl { get; set; } = default!;
    }
}
