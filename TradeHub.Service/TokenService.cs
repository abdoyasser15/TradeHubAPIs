using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TradeHub.DTOs;
using TradeHub.Repository;
using TradHub.Core.Entity.Identity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public TokenService(IConfiguration configuration, 
            UserManager<AppUser> userManager , AppDbContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> CreateTokenAsync(AppUser User, UserManager<AppUser> userManager)
        {
            var authCalims = new List<Claim>()
            {
                new Claim("FullName",User.FullName),
                new Claim(ClaimTypes.NameIdentifier,User.Id),
                new Claim(ClaimTypes.Email,User.Email),
                new Claim(ClaimTypes.MobilePhone,User.PhoneNumber),
                new Claim("AccountType",User.AccountType.ToString() ?? string.Empty)
            };
            var userRoles = await userManager.GetRolesAsync(User);
            foreach (var userRole in userRoles)
            {
                authCalims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                claims: authCalims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow
            };
        }

        public async Task<UserDto?> RefreshTokenAsync(string token)
        {
            var userDto = new UserDto();

            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null) return null;

            var refreshToken = _context.RefreshTokens.SingleOrDefault(t => t.Token == token);
            if (refreshToken is null || !refreshToken.IsActive) return null;
            refreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = await GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var tokenString = await CreateTokenAsync(user, _userManager);
            return userDto = new UserDto
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = (List<string>)await _userManager.GetRolesAsync(user),
                Token = tokenString,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var userDto = new UserDto();

            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null) 
                return false;

            var refreshToken = _context.RefreshTokens.SingleOrDefault(t => t.Token == token);
            if (refreshToken is null || !refreshToken.IsActive) return false;
            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }
    }
}
