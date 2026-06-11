using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Settings
{
    public class PaymobSettings
    {
        public string BaseUrl { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string PublicKey { get; set; } = default!;
        public string HmacSecret { get; set; } = default!;
        public string Currency { get; set; } = "EGP";
        public int IntegrationId { get; set; }
        public int IframeId { get; set; }
        public string CallbackUrl { get; set; } = default!;
        public string ReturnUrl { get; set; } = default!;
    }
}
