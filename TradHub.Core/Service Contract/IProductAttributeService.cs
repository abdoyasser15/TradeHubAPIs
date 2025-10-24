using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface IProductAttributeService
    {
        Task<IReadOnlyList<TradHub.Core.Dtos.productattributedto>> GetAllProductAttribute();
        Task<TradHub.Core.Dtos.productattributedto> GetProductAttributeById(int ProductAttributeId);
        Task<bool> CreateProductAttributesAsync(CreateProductAttribute ProductAttribute);
        Task<bool> UpdateProductAttributesAsync(int productAttributeId, CreateProductAttribute dto);
        Task<bool> DeleteProductAttributesAsync(int productAttributeId);
    }
}
