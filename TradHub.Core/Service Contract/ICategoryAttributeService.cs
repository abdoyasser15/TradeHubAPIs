using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface ICategoryAttributeService
    {
        Task<IReadOnlyList<CategoryAttributeDto>> GetAllCategoryAttribute();
        Task<CategoryAttributeDto> GetCategoryAttributeById(int id);
        Task<bool> AddCategoryAttribute(CategoryAttributeCreateDto dto);
        Task<bool> UpdateCategoryAttribute(int id, CategoryAttributeUpdateDto dto);
        Task<bool> DeleteCategoryAttribute(int id);
    }
}
