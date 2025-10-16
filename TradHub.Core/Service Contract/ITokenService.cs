using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.DTOs;
using TradHub.Core.Entity.Identity;

namespace TradHub.Core.Service_Contract
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser User, UserManager<AppUser> userManager);
        Task<RefreshToken> GenerateRefreshToken();
        Task<UserDto?> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
