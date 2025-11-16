using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service.Products.Command.Create_Product
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CreateProductHandler(IUnitOfWork unitOfWork , IMapper mapper, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("Mapping CreateProductDto to Product entity");

                var product = _mapper.Map<Product>(request.Product);

                _logger.LogInfo("Adding product to database");

                var createdProduct = _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.CompleteAsync();

                _logger.LogInfo("Product added successfully with Id={Id}", product.Id);

                return _mapper.Map<ProductDto>(product);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                throw;
            }
        }
    }
}
