using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradHub.Core.Service_Contract
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllAsync();
        Task<IReadOnlyList<CategoryDto>> GetActiveAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto?> AddAsync(CategoryDto category);
        Task<CategoryDto?> UpdateAsync(int id, CategoryDto category);
        Task<bool> DeleteAsync(int id);
    }
}
