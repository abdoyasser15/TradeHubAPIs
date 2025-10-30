using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using TradeHub.DTOs;
using TradeHub.Errors;
using TradeHub.Repository;
using TradeHub.Service;
using TradeHub.Service.Errors;
using TradHub.Core;
using TradHub.Core.Dtos;
using TradHub.Core.Entity;
using TradHub.Core.Entity.Identity;
using TradHub.Core.Enums;
using TradHub.Core.Repository_Contract;
using TradHub.Core.Service_Contract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TradeHub.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IOtpService _otpService;

        public AccountController(IAuthService authService, 
            UserManager<AppUser> userManager , 
            SignInManager<AppUser> signInManager, 
            ITokenService tokenService , 
            IConfiguration configuration,
            IOtpService otpService)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _otpService = otpService;
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser is null)
                return Unauthorized(new ApiResponse(401, "Invalid Email Or Password"));

            if (!existingUser.EmailConfirmed)
                return Unauthorized(new ApiResponse(401, "Please confirm your email before logging in."));

            var result = await _signInManager.CheckPasswordSignInAsync(existingUser, model.Password, true);
            if (result.IsLockedOut)
                return StatusCode(423, new ApiResponse(423, "User Account is Locked"));

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401, "Invalid Email Or Password"));
            var user = existingUser.AccountType == AccountType.Business
                ? await _authService.LoginInBusiness(existingUser)
                : await _authService.LoginInIndividual(existingUser);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "Problem Logging In"));
            if(!string.IsNullOrEmpty(user.RefreshToken))
                setRefreshTokenInCookie(user.RefreshToken, user.RefreshTokenExpiration);
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return BadRequest(new ApiResponse(400, "Email already in use"));

            var user = model.AccountType == AccountType.Business
            ? await _authService.RegisterBusinessAsync(model)
            : await _authService.RegisterIndividualAsync(model);

            if (user is null)
                return BadRequest(new ApiResponse(400, "Problem Registering"));
            return Ok(user);
        }
        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ApiResponse(401));

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is null)
                return Unauthorized(new ApiResponse(401));
            var userDto = await _authService.GetCurrentUserAsync(existingUser);
            return Ok(userDto);
        }
        [Authorize]
        [HttpPut("update-profile")]
        public async Task<ActionResult<ReturnUpdateUserDto>> UpdateUser(UpdateUserDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound(new ApiResponse(401, "User Not Found"));
            
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Problem Updating User Info"));
            return Ok(new ReturnUpdateUserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            });
        }
        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<ActionResult> DeleteAccount(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound(new ApiResponse(404, "User Not Found"));
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Problem Deleting User Account"));
            return Ok(new ApiResponse(200, "User Account Deleted Successfully"));
        }
        [HttpGet("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new ApiResponse(401, "You are not authorized"));
            var user = await _tokenService.RefreshTokenAsync(refreshToken);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "You are not authorized"));
            if (!string.IsNullOrEmpty(user.RefreshToken))
                setRefreshTokenInCookie(user.RefreshToken, user.RefreshTokenExpiration);
            return Ok(user);
        }
        [HttpPost("revoke-token")]
        public async Task<ActionResult> RevokeToken([FromBody] RevokeTokenDto model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest(new ApiResponse(400, "Token is required"));
            var result = await _tokenService.RevokeTokenAsync(token);
            if (!result)
                return NotFound(new ApiResponse(404, "Token not found"));
            return Ok(new ApiResponse(200, "Token revoked"));
        }
        [HttpPost("logout")]
        public async Task<ActionResult> Logout([FromBody] RevokeTokenDto model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest(new ApiResponse(400, "Token is required"));
            var result = await _tokenService.RevokeTokenAsync(token);
            if (!result)
                return NotFound(new ApiResponse(404, "Token not found"));
            Response.Cookies.Delete("refreshToken");
            return Ok(new ApiResponse(200, "Logged out successfully"));
        }
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return Ok(new { message = "If the email is registered, a reset link has been sent." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action(
                action: "ResetPassword", 
                controller: "Account",    
                values: new { token, email = user.Email },
                protocol: Request.Scheme 
            );
            return Ok(new { message = "Reset link sent to your email." });
        }
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data." });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { message = "Invalid request." });

            var verified = await _otpService.ValidateOtpAsync(model.Email, model.otpCode);

            if(!verified)
                return BadRequest(new { message = "Invalid or expired OTP code." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new
                {
                    message = "Password reset failed.",
                    errors
                });
            }
            return Ok(new { message = "Password reset successfully." });
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400,"Invalid input data."));
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ApiResponse(401, "Invalid or missing email claim."));
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiResponse(404,"User Not Found!"));
            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new
                {
                    message = "Failed to change password.",
                    errors
                });
            }
            return Ok(new ApiResponse(200, "Password changed successfully."));
        }

        private void setRefreshTokenInCookie(string refreshToken, DateTime Expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = Expires.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        [HttpPost("google")]
        public async Task<ActionResult<UserIndividualDto>> LoginWithGoogle([FromBody] GoogleLoginDto dto)
        {
            var googleUser = await GetGoogleUserAsync(dto.AccessToken);
            if (googleUser is null)
                return Unauthorized("Invalid Google token");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u =>  u.Email == googleUser.Email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = googleUser.Email ?? googleUser.Id,
                    Email = googleUser.Email,
                    FirstName = googleUser.Name?.Split(' ').FirstOrDefault(),
                    LastName = googleUser.Name?.Split(' ').Skip(1).FirstOrDefault(),
                    AccountType = AccountType.Individual,
                    Role = UserRole.User,
                    EmailConfirmed = true,
                    LoginProvider = "Google",
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, user.Role.ToString());
            }
            else
            {
                user.FirstName = googleUser.Name?.Split(' ').FirstOrDefault();
                user.LastName = googleUser.Name?.Split(' ').Skip(1).FirstOrDefault();
                user.LoginProvider = "Google";
                await _userManager.UpdateAsync(user);
            }
            var login = await _authService.LoginInIndividual(user);
            return Ok(login);
        }
        private async Task<GoogleUserInfo?> GetGoogleUserAsync(string accessToken)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={accessToken}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GoogleUserInfo>(json);
        }

        [HttpPost("facebook")]
        public async Task<ActionResult<UserIndividualDto>> LoginWithFacebook([FromBody] FacebookLoginDto dto)
        {
            var fbUser = await GetFacebookUserAsync(dto.AccessToken);
            if (fbUser is null) return Unauthorized("Invalid Facebook token");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == fbUser.Email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = fbUser.Email ?? fbUser.Id,
                    Email = fbUser.Email,
                    FirstName = fbUser.Name?.Split(' ').FirstOrDefault(),
                    LastName = fbUser.Name?.Split(' ').Skip(1).FirstOrDefault(),
                    AccountType = AccountType.Individual,
                    Role = UserRole.User,
                    EmailConfirmed = true ,
                    LoginProvider = "Facebook",
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, user.Role.ToString());
            }

            var login =  await _authService.LoginInIndividual(user);
            return Ok(login);
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest model)
        {
            var result = await _otpService.GenerateOtpAsync(model.PhoneOrEmail);

            if (!result)
                return BadRequest(new { message = "Failed to send OTP" });

            return Ok(new { message = "OTP sent successfully" });
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var isValid = await _otpService.ValidateOtpAsync(request.PhoneOrEmail, request.Code);

            if (!isValid)
                return BadRequest(new { message = "Invalid or expired OTP" });

            return Ok(new { message = "OTP verified successfully" });
        }
        [HttpPost("verify-account")]
        public async Task<ActionResult> VerifyAccount([FromBody] VerifyAccountDto model)
        {
            AppUser? user = null;
            if (!string.IsNullOrEmpty(model.Email))
                user = await _userManager.FindByEmailAsync(model.Email);
            else if (!string.IsNullOrEmpty(model.Phone))
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.Phone);
            if (user is null)
                return NotFound(new ApiResponse(404, "User Not Found"));
            if (!string.IsNullOrEmpty(model.Email))
                user.EmailConfirmed = true;
            if (!string.IsNullOrEmpty(model.Phone))
                user.PhoneNumberConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Problem Verifying Account"));
            return Ok(new ApiResponse(200, "Account Verified Successfully"));
        }
        private async Task<FacebookUserInfo?> GetFacebookUserAsync(string accessToken)
        {
            using var client = new HttpClient();
            var url = $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookUserInfo>(json);
        }

    }
}
