using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        AwaitingPayment = 1,
        Confirmed = 2,
        Processing = 3,
        Shipped = 4,
        Delivered = 5,
        Cancelled = 6,
        Refunded = 7,
        Failed = 8
    }
}
