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
    public class GetProductsQuery : IRequest<Pagination<ProductDto>>
    {
        public ProductSpecParams ProductSepc { get; set; }
        public GetProductsQuery(ProductSpecParams productspec) 
        {
            ProductSepc = productspec;
        }
    }
}
