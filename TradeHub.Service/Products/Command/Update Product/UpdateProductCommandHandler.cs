using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;

namespace TradeHub.Service.Products.Command.Update_Product
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(request.Id);
            if (product is null) return false;
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
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
