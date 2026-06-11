using MediatR;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Specifications.Product_Spec;

namespace TradeHub.Service.Products.Queries
{
    public record GetProductsBySubCategoryIdQuery(int subCategoryId) : IRequest<IReadOnlyList<ProductDto>>;
}