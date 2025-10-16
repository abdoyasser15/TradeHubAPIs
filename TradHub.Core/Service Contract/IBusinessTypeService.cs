using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;

namespace TradHub.Core.Service_Contract
{
    public interface IBusinessTypeService
    {
        Task<IReadOnlyList<BusinessTypeDto>> GetBusinessTypesAsync();
        Task<BusinessTypeDto> GetBusinessTypeByIdAsync(int id);
        Task<BusinessTypeDto?> AddBusinessTypeAsync(BusinessTypeDto businessType);
        Task<BusinessTypeDto?> UpdateBusinessTypeAsync(int Id,BusinessTypeDto businessType);
        Task<bool> DeleteBusinessTypeAsync(int id);
    }
}
