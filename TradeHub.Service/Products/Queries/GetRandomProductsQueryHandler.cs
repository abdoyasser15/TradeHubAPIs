using MediatR;
using TradeHub.Service.Products.Queries;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Specifications.Product_Spec;

public class GetRandomProductsQueryHandler
    : IRequestHandler<GetRandomProductsQuery, Pagination<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRandomProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Pagination<ProductDto>> Handle(
        GetRandomProductsQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new RandomProductSpecification(request.ProductSepc);

        var products = await _unitOfWork.Repository<Product>()
            .GetAllSpecificationsAsync(spec);

        var countSpec = new ProductCountSpecifications(request.ProductSepc);

        var totalItems = await _unitOfWork.Repository<Product>()
            .CountAsync(countSpec);

        var productDtos = products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name!,
                Description = p.Description!,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,

                SubCategoryId = p.SubCategoryId,
                SubCategoryName = p.SubCategory.Name,

                CompanyId = p.CompanyId,
                CompanyName = p.Company.BusinessName,
                LogoUrl = p.Company.LogoUrl!,

                CategoryId = p.SubCategory.CategoryId,
                CategoryName = p.SubCategory.Category.Name,

                Attributes = p.ProductAttributes.Select(pa => new productattributedto
                {
                    Id = pa.Id,
                    CategoryAttributeId = pa.CategoryAttributeId,
                    CategoryAttributeName = pa.CategoryAttribute.Name,
                    Value = pa.Value!
                }).ToList(),

                AverageRating = p.ProductRatings.Any()
                    ? p.ProductRatings.Average(r => r.RaitngValue)
                    : 0,

                RatingCount = p.ProductRatings.Count(),

                IsFavourite = request.UserId != null &&
                    p.Favourites.Any(f => f.UserId == request.UserId)
            })
            .ToList();

        return new Pagination<ProductDto>
        (
            request.ProductSepc.pageIndex,
            request.ProductSepc.PageSize,
            totalItems,
            productDtos
        );
    }
}