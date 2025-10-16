using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.DTOs;
using TradHub.Core.Dtos;
using TradHub.Core.Entity.Identity;
using TradHub.Core.Enums;
using TradHub.Core.Service_Contract;

namespace TradeHub.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ReturnUserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.Users.Where(u=>u.Id==id).FirstOrDefaultAsync();
            if(user is null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return new ReturnUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };
        }
        public async Task<IReadOnlyList<ReturnUserDto?>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.Where(a=>a.AccountType==AccountType.Individual)
                .ToListAsync();
            var result = new List<ReturnUserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new ReturnUserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                });
            }
            return result;
        }
        

        public async Task<bool?> UpdateUserRoleAsync(UpdateUserRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user is null) return null;

            var roleExists = await _roleManager.RoleExistsAsync(model.NewRole);
            if (!roleExists) return false;

            var currentRole = await _userManager.GetRolesAsync(user);
            if (currentRole.Count == 1 && currentRole.Contains(model.NewRole))
                return true;
            var removeRole =  await _userManager.RemoveFromRolesAsync(user, currentRole);
            if(!removeRole.Succeeded) return false;
            var result = await _userManager.AddToRoleAsync(user, model.NewRole);
            return result.Succeeded;
        }

        public async Task<bool?> DeleteUserAsync(string id)
        {
            var user = await _userManager.Users.Where(u=>u.Id==id).FirstOrDefaultAsync();
            if (user is null)
                return null;
            var result = await _userManager.DeleteAsync(user);
            if(!result.Succeeded) return false; 
            return true;
        }
    }
}
