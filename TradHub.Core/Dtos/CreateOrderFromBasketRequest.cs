using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CreateOrderFromBasketRequest
    {
        public int BasketId { get; set; }
        public double DeliveryFee { get; set; }
        public string? Address { get; set; }
    }
}
