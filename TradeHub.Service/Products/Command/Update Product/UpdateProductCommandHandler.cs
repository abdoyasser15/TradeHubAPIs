using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service.Products.Command.Update_Product
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IImageService _imageService;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imageService = imageService;
        }
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Updating product with Id={Id}", request.Id);

            var product = await _unitOfWork.Repository<Product>().GetById(request.Id);

            if (product is null)
            {
                _logger.LogWarn("Product with Id={Id} not found", request.Id);
                throw new KeyNotFoundException($"Product with Id {request.Id} not found");
            }

            var dto = request.ProductDto;

            if (dto.CategoryId > 0)
            {
                var categoryExists = await _unitOfWork.Repository<SubCategory>()
                    .GetById(dto.CategoryId);

                if (categoryExists is null)
                    throw new ArgumentException("Invalid CategoryId");
            }

            if (dto.CompanyId != Guid.Empty)
            {
                var companyExists = await _unitOfWork.Repository<Company>()
                    .GetById(dto.CompanyId);

                if (companyExists is null)
                    throw new ArgumentException("Invalid CompanyId");
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;
            product.SubCategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;
            product.CompanyId = dto.CompanyId;

            if (dto.ImageUrl is not null)
            {
                if (!string.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    _imageService.DeleteImage(product.ImageUrl);
                }

                product.ImageUrl = await _imageService.UploadImageAsync(
                    dto.ImageUrl,
                    "images/products"
                );
            }

            _unitOfWork.Repository<Product>().Update(product);

            var affectedRows = await _unitOfWork.CompleteAsync();

            if (affectedRows <= 0)
                throw new InvalidOperationException("Update operation failed");

            _logger.LogInfo("Product with Id={Id} updated successfully", request.Id);
        }
    }
}
