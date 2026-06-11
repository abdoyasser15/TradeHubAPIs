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
        private readonly IImageService _imageService;

        public CreateProductHandler(IUnitOfWork unitOfWork , IMapper mapper, ILoggerManager logger,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _imageService = imageService;
        }
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("Mapping CreateProductDto to Product entity");

                var product = _mapper.Map<Product>(request.Product);

                var imageUrl = await _imageService.UploadImageAsync(
                    request.Product.Image!,
                    "images/products"
                );

                product.ImageUrl = imageUrl;

                _logger.LogInfo("Adding product to database");

                await _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.CompleteAsync();

                _logger.LogInfo("Product added successfully with Id={Id}", product.Id);

                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                throw;
            }
        }
    }
}
