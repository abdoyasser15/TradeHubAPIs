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
using TradHub.Core.Specifications.ProductAttributeSpec;

namespace TradeHub.Service
{
    public class ProductAttributeService : IProductAttributeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public ProductAttributeService(IUnitOfWork unitOfWork , ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<productattributedto> GetProductAttributeById(int ProductAttributeId)
        {
            try
            {
                _logger.LogInfo("Fetching product attribute with ID: {ProductAttributeId}",ProductAttributeId);
                var ProductAttributeSpec = new ProductAttributeWithCategory(ProductAttributeId);
                var productAttribute = await _unitOfWork.Repository<ProductAttribute>().GetWithSpecAsync(ProductAttributeSpec);
                if (productAttribute == null) {
                    _logger.LogWarn("Product attribute with ID: {ProductAttributeId} not found.",ProductAttributeId);
                    return null!;
                }
                _logger.LogInfo("Product attribute with ID: {ProductAttributeId} fetched successfully.",ProductAttributeId);
                return new productattributedto
                {
                    Id = productAttribute.Id,
                    CategoryAttributeId = productAttribute.CategoryAttributeId,
                    CategoryAttributeName = productAttribute.CategoryAttribute.Name,
                    Value = productAttribute.Value
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occurred while fetching product attribute with ID: {ProductAttributeId}. Error: {ErrorMessage}",ex.Message);
                throw;
            }
        }
        public async Task<IReadOnlyList<productattributedto>> GetAllProductAttribute()
        {
            try
            {
                _logger.LogInfo("Fetching all product attributes.");
                var ProductAttributeSpec = new ProductAttributeWithCategory();
                var productAttributes = await _unitOfWork.Repository<ProductAttribute>().GetAllSpecificationsAsync(ProductAttributeSpec);
                _logger.LogInfo("Total product attributes fetched: {Count}",productAttributes.Count);
                return productAttributes.Select(pa => new productattributedto
                {
                    Id = pa.Id,
                    CategoryAttributeId = pa.CategoryAttributeId,
                    CategoryAttributeName = pa.CategoryAttribute.Name,
                    Value = pa.Value
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occurred while fetching all product attributes. Error: {ErrorMessage}",ex.Message);
                throw;
            }
        }
        public async Task<bool> CreateProductAttributesAsync(CreateProductAttribute ProductAttribute)
        {
            try
            {
                _logger.LogInfo("Creating a new product attribute for Product ID: {ProductId}",ProductAttribute.ProductId);
                var productAttribute = new ProductAttribute
                {
                    ProductId = ProductAttribute.ProductId,
                    CategoryAttributeId = ProductAttribute.CategoryAttributeId,
                    Value = ProductAttribute.Value
                };
                await _unitOfWork.Repository<ProductAttribute>().AddAsync(productAttribute);
                await _unitOfWork.CompleteAsync();
                _logger.LogInfo("Product attribute created successfully for Product ID: {ProductId}",ProductAttribute.ProductId);
                return true;
            }
            catch(SqlException ex)
            {
                _logger.LogError(ex,"SQL error occurred while creating a new product attribute. Error: {ErrorMessage}",ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occurred while creating a new product attribute. Error: {ErrorMessage}",ex.Message);
                throw;
            }
        }
        public async Task<bool> UpdateProductAttributesAsync(int productAttributeId , CreateProductAttribute dto)
        {
            try
            {
                _logger.LogInfo("Updating product attribute with ID: {ProductAttributeId}", productAttributeId);
                var productAttrubute = await _unitOfWork.Repository<ProductAttribute>().GetById(productAttributeId);
                if (productAttrubute is null)
                {
                    _logger.LogWarn("Product attribute with ID: {ProductAttributeId} not found for update.", productAttributeId);
                    return false;
                }
                productAttrubute.ProductId = dto.ProductId;
                productAttrubute.CategoryAttributeId = dto.CategoryAttributeId;
                productAttrubute.Value = dto.Value;
                var updatedProductAttribute = productAttrubute;
                _unitOfWork.Repository<ProductAttribute>().Update(updatedProductAttribute);
                await _unitOfWork.CompleteAsync();
                _logger.LogInfo("Product attribute with ID: {ProductAttributeId} updated successfully.", productAttributeId);
                return true;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while creating a new product attribute. Error: {ErrorMessage}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product attribute with ID: {ProductAttributeId}. Error: {ErrorMessage}", productAttributeId, ex.Message);
                throw;
            }
        }
        public async Task<bool> DeleteProductAttributesAsync(int productAttributeId)
        {
            try
            {
                _logger.LogInfo("Deleting product attribute with ID: {ProductAttributeId}", productAttributeId);
                var productAttribute = await _unitOfWork.Repository<ProductAttribute>().GetById(productAttributeId);
                if (productAttribute == null) {
                    _logger.LogWarn("Product attribute with ID: {ProductAttributeId} not found for deletion.", productAttributeId);
                    return false;
                }
                _unitOfWork.Repository<ProductAttribute>().DeleteAsync(productAttribute);
                await _unitOfWork.CompleteAsync();
                _logger.LogInfo("Product attribute with ID: {ProductAttributeId} deleted successfully.", productAttributeId);
                return true;
            }
            catch(SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while deleting product attribute with ID: {ProductAttributeId}. Error: {ErrorMessage}", productAttributeId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product attribute with ID: {ProductAttributeId}. Error: {ErrorMessage}", productAttributeId, ex.Message);
                throw;
            }
        }
    }
}
