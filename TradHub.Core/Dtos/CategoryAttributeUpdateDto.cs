using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class CategoryAttributeUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; }
        public bool IsRequired { get; set; }
        public int CategoryId { get; set; }
    }
}
