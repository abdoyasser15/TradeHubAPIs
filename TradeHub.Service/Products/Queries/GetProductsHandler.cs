using AutoMapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.Product_Spec;

namespace TradeHub.Service.Products.Queries
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, Pagination<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public GetProductsHandler(IUnitOfWork unitOfWork , IMapper mapper , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Pagination<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("Fetching products with filters: {@Filters}", request.ProductSepc);

                var spec = new ProductSpecification(request.ProductSepc);
                var products = await _unitOfWork.Repository<Product>().GetAllSpecificationsAsync(spec);

                var countSpec = new ProductCountSpecifications(request.ProductSepc);
                var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

                _logger.LogInfo("Fetched {Count} products from DB", products.Count);

                var mappedProducts = _mapper.Map<IReadOnlyList<ProductDto>>(products);

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
}
