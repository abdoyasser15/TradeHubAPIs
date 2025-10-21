using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface ICompanyCategoryService
    {
        Task<IReadOnlyList<CompanyCategoryDto>> GetByCompanyIdAsync(Guid companyId);
        Task<bool> AddAsync(CompanyCategoryCreateDto dto);
        Task<bool> RemoveAsync(Guid companyId, int categoryId);
    }
}
