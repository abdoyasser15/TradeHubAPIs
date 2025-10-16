using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TradeHub.DTOs;
using TradHub.Core.Dtos;
using TradHub.Core.Entity.Identity;

namespace TradHub.Core.Service_Contract
{
    public interface IAuthService
    {
        Task<UserIndividualDto?> RegisterIndividualAsync(RegisterDto model);
        Task<UserBusinessDto?> RegisterBusinessAsync(RegisterDto model);
        Task<UserDto?> GetCurrentUserAsync(AppUser User);
        Task<UserBusinessDto> LoginInBusiness(AppUser appUser);
        Task<UserIndividualDto> LoginInIndividual(AppUser appUser);
    }
}
