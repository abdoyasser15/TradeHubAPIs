using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CreateProductOptionValueDto
    {
        public string Name { get; set; } = null!;
        public double ExtraPrice { get; set; }
    }
}
