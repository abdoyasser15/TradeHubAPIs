using AutoMapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork , IMapper mapper , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Getting product with id: {request.Id}");
                var spec = new ProductSpecification(request.Id);
                var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
                if (product is null) {
                    _logger.LogWarn("Product with Id={Id} not found", request.Id);
                    return null;
                }
                _logger.LogInfo("Product with Id={Id} fetched successfully", request.Id);
                return _mapper.Map<ProductDto>(product);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching product with Id={Id}", request.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching product with Id={Id}", request.Id);
                throw;
            }
        }
    }
}
