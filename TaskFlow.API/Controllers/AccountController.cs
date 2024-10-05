using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Service.DTOs;
using Service.DTOs.Auth;
using Service.DTOs.Result;
using Service.Interfaces;
using Service.Utility;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController(UserManager<AppUser> userManager, IAccountService accountService) : BaseController
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IAccountService _accountService = accountService;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginAttemptDto loginAttempt)
        {
            var modelStateErorr = GetModelStateError();
            if (modelStateErorr != null)
                return ErrorResult(modelStateErorr);

            ServiceResult<UserDto> serviceResult = await _accountService.LoginAsync(loginAttempt);
            return HandleServiceResult(serviceResult);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterAttemptDto registerAttempt)
        {
            var modelStateErorr = GetModelStateError();
            if (modelStateErorr != null)
                return ErrorResult(modelStateErorr);

            var serviceResult = await _accountService.RegisterAsync(registerAttempt);
            if (serviceResult.Data != null && !serviceResult.IsError) await SetRefreshToken(serviceResult.Data);
            return HandleServiceResult(serviceResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue("email");
            if (userEmail == null) return ErrorResult(ErrorDescriber.Unauthenticated());

            ServiceResult<UserDto> serviceResult = await _accountService.GetUserByEmail(userEmail);
            return HandleServiceResult(serviceResult);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users.Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user == null) return ErrorResult(ErrorDescriber.Unauthenticated());

            ServiceResult<string> serviceResult = _accountService.RefreshToken(user, refreshToken);
            if (serviceResult.IsError) return HandleServiceResult(serviceResult);

            var userDto = new UserDto
            {
                Token = serviceResult.Data,
                UserName = user.UserName
            };

            return Ok(userDto);
        }

        private async Task SetRefreshToken(UserDto user)
        {
            if (user.UserName == null) return;
            var refreshToken = await _accountService.SetRefreshToken(user.UserName);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
