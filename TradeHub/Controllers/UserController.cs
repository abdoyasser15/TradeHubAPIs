using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeHub.Errors;
using TradHub.Core.Dtos;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user is null)
                return NotFound(new { Message = "User not found" });
            return Ok(user);
        }
        [HttpGet("all-users")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users is null || !users.Any())
                return NotFound(new { Message = "No users found" });
            return Ok(users);
        }
        [HttpDelete("delete-user")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest(new ApiResponse(400,"Invalid user Id"));
            var result = await _userService.DeleteUserAsync(id);

            if(result is null)
                return NotFound(new ApiResponse(404,"User not found"));

            if(result==false)
                return BadRequest(new ApiResponse(400,"Failed to delete user"));

            return Ok(new ApiResponse(200,"User Deleted Successfully"));
        }
        [HttpPut("update-user-role")]
        public async Task<ActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.NewRole))
                return BadRequest(new ApiResponse(400, "Invalid user Id or role"));
            var result = await _userService.UpdateUserRoleAsync(model);
            if (result is null)
                return NotFound(new ApiResponse(404, "User not found"));
            if (result == false)
                return BadRequest(new ApiResponse(400, "Failed to update user role"));
            return Ok(new ApiResponse(200, "User role updated successfully"));
        }
    }
}
