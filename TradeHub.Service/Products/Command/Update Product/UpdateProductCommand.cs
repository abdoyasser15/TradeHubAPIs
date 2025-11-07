using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradeHub.Service.Products.Command.Update_Product
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public UpdateProductDto ProductDto { get; set; }

    }
}
