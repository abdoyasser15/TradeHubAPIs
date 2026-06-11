using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface IProductOptionsService
    {
        Task<IReadOnlyList<ProductOptionDto>> GetOptionsByProductIdAsync(int productId);
        Task<ProductOptionDto?> AddOptionToProductAsync(int productId, CreateProductOptionDto dto);
        Task<bool> DeleteOptionAsync(int optionId);
    }
}
