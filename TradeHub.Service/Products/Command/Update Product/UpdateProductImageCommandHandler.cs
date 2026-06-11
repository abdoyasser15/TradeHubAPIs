using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service.Products.Command.Update_Product
{
    public class UpdateProductImageCommandHandler
    : IRequestHandler<UpdateProductImageCommand, string?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly ILoggerManager _logger;

        public UpdateProductImageCommandHandler(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<string?> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>()
                .GetById(request.ProductId);

            if (product is null)
            {
                _logger.LogWarn("Product not found Id={Id}", request.ProductId);
                return null;
            }

            if (!string.IsNullOrWhiteSpace(product.ImageUrl))
            {
                _imageService.DeleteImage(product.ImageUrl);
            }

            var newImageUrl = await _imageService.UploadImageAsync(
                request.Image,
                "images/products"
            );

            product.ImageUrl = newImageUrl;

            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompleteAsync();

            return newImageUrl;
        }
    }
}
