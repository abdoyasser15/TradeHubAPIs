using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradeHub.Service.Products.Command.Create_Product
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public CreateProductDto Product { get; set; }
        public CreateProductCommand(CreateProductDto product)
        {
            Product = product;
        }
    }
}
