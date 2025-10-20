using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TradeHub.DTOs;
using TradeHub.Errors;
using TradeHub.Repository;
using TradeHub.Service.Errors;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Entity.Identity;
using TradHub.Core.Enums;
using TradHub.Core.Service_Contract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TradeHub.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext context;

        public AuthService(UserManager<AppUser> userManager , 
            IUnitOfWork unitOfWork , ITokenService tokenService , AppDbContext context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            this.context = context;
        }
        public async Task<UserBusinessDto?> RegisterBusinessAsync(RegisterDto model)
        {
            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split("@")[0],
                AccountType = model.AccountType,
                Role = UserRole.CompanyOwner,
                CreatedAt = DateTime.UtcNow,
                LoginProvider = model.LoginProvider ?? "Local"
            };
            if (!model.LocationId.HasValue || !model.BusinessTypeId.HasValue)
                throw new ApiValidationException(new[] { "LocationId and BusinessTypeId are required for business accounts." });
            var company = new Company
            {
                BusinessName = model.BusinessName,
                LocationId = model.LocationId!.Value,
                LogoUrl = model.LogoUrl,
                BusinessTypeId = model.BusinessTypeId!.Value,
                CreatedBy = user
            };

            await _unitOfWork.Repository<Company>().AddAsync(company);
            await _unitOfWork.CompleteAsync();
            user.CompanyId = company.CompanyId;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new ApiValidationException(errors);
            }

            await _userManager.AddToRoleAsync(user, user.Role.ToString());
            var token = await _tokenService.CreateTokenAsync(user, _userManager);

            return new UserBusinessDto
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = (List<string>)await _userManager.GetRolesAsync(user),
                Token = token,
                CompanyId = company.CompanyId,
                BusinessType = company.BusinessTypeId,
                BusinessName = company.BusinessName,
                LoginProvider = user.LoginProvider
            };
        }

        public async Task<UserIndividualDto?> RegisterIndividualAsync(RegisterDto model)
        {
            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split("@")[0],
                AccountType = model.AccountType,
                Role = model.Role == UserRole.Admin ? UserRole.Admin : UserRole.User,
                LoginProvider = model.LoginProvider ?? "Local",
                CreatedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, model.Password); 
            if (!result.Succeeded) 
            { 
                var errors = result.Errors.Select(e => e.Description);
                throw new ApiValidationException(errors); 
            }
            await _userManager.AddToRoleAsync(user, user.Role.ToString());
            var token = await _tokenService.CreateTokenAsync(user, _userManager);
            return new UserIndividualDto
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = (List<string>)await _userManager.GetRolesAsync(user),
                LoginProvider = user.LoginProvider,
                Token = token
            };
        }
        public async Task<UserDto?> GetCurrentUserAsync(AppUser User)
        {
            var roles = (await _userManager.GetRolesAsync(User)).ToList();
            var token = await _tokenService.CreateTokenAsync(User, _userManager);

            return new UserDto
            {
                UserName = User.UserName,
                FirstName = User.FirstName,
                LastName = User.LastName,
                FullName = User.FullName,
                Email = User.Email,
                PhoneNumber = User.PhoneNumber,
                Roles = roles,
                Token = token
            };
        }

        public async Task<UserBusinessDto> LoginInBusiness(AppUser user)
        {

            var token = await _tokenService.CreateTokenAsync(user, _userManager);
            var roles = await _userManager.GetRolesAsync(user);
            var company = await _unitOfWork.Repository<Company>().GetById(user.CompanyId);


            var userDto = new UserBusinessDto
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList(),
                Token = token,
                CompanyId = company?.CompanyId,
                BusinessType = company?.BusinessTypeId,
                BusinessName = company?.BusinessName,
                LoginProvider = user.LoginProvider
            };
            var User = await context.Users.Include(u=>u.RefreshTokens).FirstOrDefaultAsync(u=>u.Email==user.Email);
            bool condition = User != null && User.RefreshTokens != null && User.RefreshTokens.Any(t => t.IsActive);
            if (condition)
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                userDto.RefreshToken = activeRefreshToken.Token;
                userDto.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = await _tokenService.GenerateRefreshToken();
                userDto.RefreshToken = refreshToken.Token;
                userDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }
            return userDto;
        }

        public async Task<UserIndividualDto> LoginInIndividual(AppUser user)
        {
            var token = await _tokenService.CreateTokenAsync(user, _userManager);
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserIndividualDto
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList(),
                Token = token,
                LoginProvider = user.LoginProvider
            };
            var User = await context.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Email == user.Email);
            bool condition = User != null && User.RefreshTokens != null && User.RefreshTokens.Any(t => t.IsActive);
            if (condition)
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                userDto.RefreshToken = activeRefreshToken.Token;
                userDto.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = await _tokenService.GenerateRefreshToken();
                userDto.RefreshToken = refreshToken.Token;
                userDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }
            return userDto;
        }

    }
}
