using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Entity.Orders
{
    public class OrderItem : BaseEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string CompanyId { get; set; } = default!;
        public ICollection<OrderItemOption> Options { get; set; } = new List<OrderItemOption>();
    }
}
