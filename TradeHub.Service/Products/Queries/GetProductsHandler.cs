using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using TradeHub.Service.Products.Queries;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.Product_Spec;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, Pagination<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetProductsHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerManager logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Pagination<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInfo("Fetching products with filters: {@Filters}", request.ProductSepc);

            var userId = _httpContextAccessor.HttpContext?.User?
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var spec = new ProductSpecification(request.ProductSepc);
            var products = await _unitOfWork.Repository<Product>().GetAllSpecificationsAsync(spec);

            var countSpec = new ProductCountSpecifications(request.ProductSepc);
            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            _logger.LogInfo("Fetched {Count} products from DB", products.Count);

            var favouriteProducts = !string.IsNullOrEmpty(userId)
             ? await _unitOfWork.Repository<Favourite>().FindAsync(f => f.UserId == userId)
             : new List<Favourite>();

            bool IsProductFavourite(int productId)
            {
                return favouriteProducts.Any(f => f.ProductId == productId);
            }
            var mappedProducts = products.Select(p => new ProductDto
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

                AverageRating = p.ProductRatings != null && p.ProductRatings.Any()
                 ? p.ProductRatings.Average(r => r.RaitngValue)
                 : 0,

                RatingCount = p.ProductRatings?.Count ?? 0,
                IsFavourite = IsProductFavourite(p.Id),

                Attributes = p.ProductAttributes?.Select(a => new productattributedto
                {
                    Id = a.Id,
                    CategoryAttributeId = a.CategoryAttributeId,
                    CategoryAttributeName = a.CategoryAttribute?.Name ?? string.Empty,
                    Value = a.Value ?? string.Empty
                }).ToList() ?? new List<productattributedto>()
            }).ToList();

            return new Pagination<ProductDto>(
                request.ProductSepc.pageIndex,
                request.ProductSepc.PageSize,
                totalItems,
                mappedProducts
            );
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error occurred while fetching products");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while fetching products");
            throw;
        }
    }
}