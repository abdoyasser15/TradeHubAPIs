using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class OrderItemOptionDto
    {
        public int ProductOptionValueId { get; set; }
        public string OptionName { get; set; } = string.Empty;
        public string ValueName { get; set; } = string.Empty;
        public double ExtraPrice { get; set; }
    }
}
