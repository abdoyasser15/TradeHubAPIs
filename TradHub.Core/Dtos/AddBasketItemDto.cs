using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradHub.Core.Dtos
{
    public class AddBasketItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public List<int> SelectedOptionValueIds { get; set; } = new();

    }
}
