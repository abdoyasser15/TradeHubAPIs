using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradeHub.Errors;
using TradHub.Core.Entity.Identity;

namespace TradeHub.Controllers
{
    public class RolesController : BaseApiController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add-role")]
        public async Task<ActionResult> AddRole([FromBody] string roleName)
        {
            if(string.IsNullOrEmpty(roleName))
                return BadRequest(new ApiResponse(400, "Role name cannot be empty"));
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return BadRequest(new ApiResponse(400, "Role already exists"));
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to create role"));
            return Ok(new { RoleName = roleName, Message = "Role created successfully" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("roles")]
        public async Task<ActionResult> GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }
    }
}
