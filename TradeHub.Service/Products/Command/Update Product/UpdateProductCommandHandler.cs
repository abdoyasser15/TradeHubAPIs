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
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("Updating product with Id={Id}", request.Id);

                var product = await _unitOfWork.Repository<Product>().GetById(request.Id);
                if (product == null)
                {
                    _logger.LogWarn("Product with Id={Id} not found", request.Id);
                    return false;
                }
                if (request.ProductDto.CategoryId > 0)
                {
                    var cat = await _unitOfWork.Repository<Category>().GetById(request.ProductDto.CategoryId);
                    if (cat is null)
                        throw new ArgumentException("Invalid CategoryId");
                }
                var UpdatedProduct = request.ProductDto;
                product.Name = UpdatedProduct.Name;
                product.Description = UpdatedProduct.Description;
                product.Price = UpdatedProduct.Price;
                product.Quantity = UpdatedProduct.Quantity;
                product.ImageUrl = UpdatedProduct.ImageUrl;
                product.CategoryId = UpdatedProduct.CategoryId;
                product.IsActive = UpdatedProduct.IsActive;
                product.CompanyId = UpdatedProduct.CompanyId;
                _unitOfWork.Repository<Product>().Update(product);
                var result = await _unitOfWork.CompleteAsync() > 0;
                return result;
            }
            catch(ArgumentException ex)
            {
                _logger.LogError(ex, "Validation error while updating product with Id={Id}", request.Id);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with Id={Id}", request.Id);
                throw;
            }
        }
    }
}
