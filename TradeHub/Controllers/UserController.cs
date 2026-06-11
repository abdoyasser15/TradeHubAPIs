using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradeHub.Errors;
using TradeHub.Service;
using TradHub.Core.Dtos;
using TradHub.Core.Entity.Identity;
using TradHub.Core.Service_Contract;

namespace TradeHub.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageService _imageService;

        public UserController(IUserService userService , 
            UserManager<AppUser> userManager, 
            IImageService imageService)
        {
            _userService = userService;
            _userManager = userManager;
            _imageService = imageService;
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
        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile image)
        {
            if (image is null)
                return BadRequest(new
                {
                    message = "Image is required"
                });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new
                {
                    message = "User not found"
                });

            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                _imageService.DeleteImage(user.ProfilePictureUrl);
            }

            var imageUrl = await _imageService.UploadImageAsync(
                image,
                "images/users"
            );

            user.ProfilePictureUrl = imageUrl;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                message = "Profile picture uploaded successfully",
                imageUrl
            });
        }

        [HttpDelete("delete-profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new
                {
                    message = "User not found"
                });

            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                _imageService.DeleteImage(user.ProfilePictureUrl);
            }

            user.ProfilePictureUrl = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                message = "Profile picture deleted successfully"
            });
        }
    }
}
