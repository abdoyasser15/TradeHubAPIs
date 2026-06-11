using MediatR;
using TradeHub.Repository;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Specifications.Product_Spec;

namespace TradeHub.Service.Products.Queries
{
    public class GetProductsBySubCategoryIdHandler
        : IRequestHandler<GetProductsBySubCategoryIdQuery, IReadOnlyList<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductsBySubCategoryIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ProductDto>> Handle(
            GetProductsBySubCategoryIdQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new ProductsBySubCategorySpecification(request.subCategoryId);

            var products = await _unitOfWork
                .Repository<Product>()
                .GetAllSpecificationsAsync(spec);

            var data = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Quantity = p.Quantity,
                SubCategoryId = p.SubCategoryId,
                SubCategoryName = p.SubCategory?.Name ?? string.Empty,
                CompanyId = p.CompanyId,
                CompanyName = p.Company?.BusinessName ?? string.Empty,
                AverageRating = p.ProductRatings.Any()
                    ? p.ProductRatings.Average(r => r.RaitngValue)
                    : 0,
                RatingCount = p.ProductRatings.Count,
                IsFavourite = false,
                Attributes = p.ProductAttributes.Select(a => new productattributedto
                {
                    Id = a.Id,
                    CategoryAttributeId = a.CategoryAttributeId,
                    CategoryAttributeName = a.CategoryAttribute?.Name ?? string.Empty,
                    Value = a.Value ?? string.Empty
                }).ToList()
            }).ToList();

            return data;
        }
    }
}