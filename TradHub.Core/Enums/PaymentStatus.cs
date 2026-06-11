using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Enums
{
    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2,
        Cancelled = 3,
        Refunded = 4,
        PartiallyRefunded = 5,
        Expired = 6,
        Authorized = 7,
        Voided = 8
    }
}
