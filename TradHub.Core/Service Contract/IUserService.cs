using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.DTOs;
using TradHub.Core.Dtos;

namespace TradHub.Core.Service_Contract
{
    public interface IUserService
    {
        Task<IReadOnlyList<ReturnUserDto?>> GetAllUsersAsync();
        Task<ReturnUserDto?> GetUserByIdAsync(string id);
        Task<bool?> UpdateUserRoleAsync(UpdateUserRoleDto model);
        Task<bool?> DeleteUserAsync(string id);
        
    }
}
