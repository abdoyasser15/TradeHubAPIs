using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto?> AddAsync(CreateCategoryDto category);
        Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto category);
        Task<bool> DeleteAsync(int id);
    }
}
