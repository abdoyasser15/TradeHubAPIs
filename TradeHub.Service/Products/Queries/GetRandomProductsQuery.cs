using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Specifications.Product_Spec;

namespace TradeHub.Service.Products.Queries
{
    public record class GetRandomProductsQuery : IRequest<Pagination<ProductDto>>
    {
        public string? UserId { get; set; }
        public ProductSpecParams ProductSepc { get; set; }
        public GetRandomProductsQuery(ProductSpecParams productspec)
        {
            ProductSepc = productspec;
        }
    }
}
