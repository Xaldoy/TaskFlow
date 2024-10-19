using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Service.DTOs.Auth;
using Service.DTOs.Result;
using System.Security.Claims;
using TaskFlow.Service.DTOs.Auth;
using TaskFlow.Service.DTOs.Error;
using TaskFlow.Service.Services.Authentication;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController(UserManager<AppUser> userManager, IAuthenticationService authenticationService) : BaseController
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginAttemptDto loginAttempt)
        {
            var modelStateErorr = GetModelStateError(MessageTypes.AuthenticationError);
            if (modelStateErorr != null)
                return MessageResult(modelStateErorr);

            ServiceResult<AuthResponseDto> serviceResult = await _authenticationService.LoginAsync(loginAttempt);
            return HandleServiceResult(serviceResult);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterAttemptDto registerAttempt)
        {
            var modelStateErorr = GetModelStateError(MessageTypes.AuthenticationError);
            if (modelStateErorr != null)
                return MessageResult(modelStateErorr);

            var serviceResult = await _authenticationService.RegisterAsync(registerAttempt);
            if (serviceResult.Data != null && !serviceResult.IsError) await SetRefreshToken(serviceResult.Data);
            return HandleServiceResult(serviceResult);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var serviceResult = _authenticationService.Logout();
            return HandleServiceResult(serviceResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (userEmail == null) return MessageResult(MessageDescriber.Unauthenticated());

            ServiceResult<AuthResponseDto> serviceResult = await _authenticationService.GetUserByEmail(userEmail);
            return HandleServiceResult(serviceResult);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users.Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user == null) return MessageResult(MessageDescriber.Unauthenticated());

            ServiceResult<string> serviceResult = _authenticationService.RefreshToken(user, refreshToken);
            if (serviceResult.IsError) return HandleServiceResult(serviceResult);

            var userDto = new AuthResponseDto
            {
                UserName = user.UserName
            };

            return Ok(userDto);
        }

        private async Task SetRefreshToken(AuthResponseDto user)
        {
            if (user.UserName == null) return;
            var refreshToken = await _authenticationService.SetRefreshToken(user.UserName);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
