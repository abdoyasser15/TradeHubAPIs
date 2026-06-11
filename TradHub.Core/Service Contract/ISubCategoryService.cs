using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradHub.Core.Service_Contract
{
    public interface ISubCategoryService
    {
        Task<IReadOnlyList<SubCategoryDto>> GetAllAsync();
        Task<IReadOnlyList<SubCategoryDto>> GetActiveAsync();
        Task<SubCategoryDto?> GetByIdAsync(int id);
        Task<SubCategoryDto?> AddAsync(CreateSubCategoryDto category);
        Task<SubCategoryDto?> UpdateAsync(int id, SubCategoryDto category);
        Task<bool> DeleteAsync(int id);
    }
}
