using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class BasketItemOptionsDto
    {
        public int ProductOptionValueId { get; set; }

        public string OptionName { get; set; } = null!;

        public string ValueName { get; set; } = null!;

        public double ExtraPrice { get; set; }


    }
}
