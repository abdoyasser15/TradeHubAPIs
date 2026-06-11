using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.Product_Spec;

namespace TradeHub.Service.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork , IMapper mapper , ILoggerManager logger, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Getting product with Id={Id}", request.Id);

            var spec = new ProductSpecification(request.Id);
            var product = await _unitOfWork
                .Repository<Product>()
                .GetWithSpecAsync(spec);

            if (product is null)
            {
                _logger.LogWarn("Product with Id={Id} not found", request.Id);
                throw new KeyNotFoundException($"Product with Id {request.Id} not found");
            }

            var userId = _httpContextAccessor.HttpContext?
                 .User?
                 .FindFirst(ClaimTypes.NameIdentifier)?
                 .Value;

            bool isFavourite = false;

            if (!string.IsNullOrEmpty(userId))
            {
                var fav = await _unitOfWork.Repository<Favourite>()
                    .FindAsync(f => f.ProductId == product.Id && f.UserId == userId);

                isFavourite = fav.Any();
            }

            var dto = _mapper.Map<ProductDto>(product);

            dto.IsFavourite = isFavourite;
            dto.AverageRating = product.ProductRatings.Any() ? product.ProductRatings.Average(r => r.RaitngValue) : 0;
            dto.RatingCount = product.ProductRatings.Count();
            _logger.LogInfo("Product with Id={Id} fetched successfully", request.Id);

            return dto;
        }
    }
}
