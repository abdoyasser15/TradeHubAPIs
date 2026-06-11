using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity.Orders
{
    public class OrderItemOption : BaseEntity
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;
        public int ProductOptionValueId { get; set; }
        public string OptionName { get; set; } = null!;
        public string ValueName { get; set; } = null!;
        public double ExtraPrice { get; set; }
    }
}
