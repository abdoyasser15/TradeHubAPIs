using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Service_Contract;
using TradHub.Core.Specifications.ProductAttributeSpec;

namespace TradeHub.Service
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductAttributeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<productattributedto> GetProductAttributeById(int ProductAttributeId)
        {
            var ProductAttributeSpec = new ProductAttributeWithCategory(ProductAttributeId);
            var productAttribute = await _unitOfWork.Repository<ProductAttribute>().GetWithSpecAsync(ProductAttributeSpec);
            if (productAttribute == null) return null!;
            var productAttributeDto = new productattributedto
            {
                Id = productAttribute.Id,
                CategoryAttributeId = productAttribute.CategoryAttributeId,
                CategoryAttributeName = productAttribute.CategoryAttribute.Name,
                Value = productAttribute.Value
            };
            return productAttributeDto;
        }
        public async Task<IReadOnlyList<productattributedto>> GetAllProductAttribute()
        {
            var ProductAttributeSpec = new ProductAttributeWithCategory();
            var productAttributes = await  _unitOfWork.Repository<ProductAttribute>().GetAllSpecificationsAsync(ProductAttributeSpec);
            var productAttributeDtos = productAttributes.Select(pa => new productattributedto
            {
                Id = pa.Id,
                CategoryAttributeId = pa.CategoryAttributeId,
                CategoryAttributeName = pa.CategoryAttribute.Name,
                Value = pa.Value
            }).ToList();
            return productAttributeDtos;
        }
        public async Task<bool> CreateProductAttributesAsync(CreateProductAttribute ProductAttribute)
        {
            var productAttribute = new ProductAttribute
            {
                ProductId = ProductAttribute.ProductId,
                CategoryAttributeId = ProductAttribute.CategoryAttributeId,
                Value = ProductAttribute.Value
            };
            _unitOfWork.Repository<ProductAttribute>().AddAsync(productAttribute);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<bool> UpdateProductAttributesAsync(int productAttributeId , CreateProductAttribute dto)
        {
            var productAttrubute = await _unitOfWork.Repository<ProductAttribute>().GetById(productAttributeId);
            if (productAttrubute == null) return false;
            productAttrubute.ProductId = dto.ProductId;
            productAttrubute.CategoryAttributeId = dto.CategoryAttributeId;
            productAttrubute.Value = dto.Value;
            var updatedProductAttribute = productAttrubute;
            _unitOfWork.Repository<ProductAttribute>().Update(updatedProductAttribute);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<bool> DeleteProductAttributesAsync(int productAttributeId)
        {
            var productAttribute = await  _unitOfWork.Repository<ProductAttribute>().GetById(productAttributeId);
            if (productAttribute == null) return false;
            _unitOfWork.Repository<ProductAttribute>().DeleteAsync(productAttribute);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
