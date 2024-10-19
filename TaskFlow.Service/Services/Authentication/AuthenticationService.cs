using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Service.DTOs.Auth;
using Service.DTOs.Result;
using TaskFlow.Service.DTOs.Auth;
using TaskFlow.Service.DTOs.Message;

namespace TaskFlow.Service.Services.Authentication
{
    public class AuthenticationService(UserManager<AppUser> userManager, TokenService tokenService, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor) : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly TokenService _tokenService = tokenService;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<ServiceResult> LoginAsync(LoginAttemptDto loginAttempt)
        {
            var user = await _userManager.FindByEmailAsync(loginAttempt.Credentials);
            user ??= await _userManager.FindByNameAsync(loginAttempt.Credentials);

            if (user == null)
            {
                return ServiceResult.Failure(MessageDescriber.UserNotFound());
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginAttempt.Password, true);

            if (result.IsLockedOut)
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                if (!lockoutEnd.HasValue)
                {
                    return ServiceResult.Failure(MessageDescriber.DefaultError());
                }
                return ServiceResult.Failure(MessageDescriber.AccountLockedOut(lockoutEnd.Value));
            }

            if (!result.Succeeded) return ServiceResult.Failure(MessageDescriber.PasswordMismatch());

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(6),
            };

            string? token = _tokenService.CreateToken(user);
            var httpContext = _httpContextAccessor.HttpContext;

            if (token == null || httpContext == null) return ServiceResult.Failure(MessageDescriber.DefaultError());

            httpContext.Response.Cookies.Append("AuthToken", token, cookieOptions);

            var userDto = new AuthResponseDto
            {
                Username = user.UserName,
            };

            return ServiceResult.Success(userDto);
        }

        public async Task<ServiceResult> RegisterAsync(RegisterAttemptDto registerAttempt)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerAttempt.Username))
                return ServiceResult.Failure(MessageDescriber.DuplicateUsername(registerAttempt.Username));

            if (await _userManager.Users.AnyAsync(x => x.Email == registerAttempt.Email))
                return ServiceResult.Failure(MessageDescriber.DuplicateEmail(registerAttempt.Email));

            var user = new AppUser
            {
                Email = registerAttempt.Email,
                UserName = registerAttempt.Username,
            };
            var result = await _userManager.CreateAsync(user, registerAttempt.Password);

            if (!result.Succeeded && result.Errors.Any())
            {
                string errorMessage = result.Errors.First().Description;
                return ServiceResult.Failure(MessageDescriber.RegistrationError(errorMessage));
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(6),
            };

            string? token = _tokenService.CreateToken(user);
            var httpContext = _httpContextAccessor.HttpContext;

            if (token == null || httpContext == null) return ServiceResult.Failure(MessageDescriber.DefaultError());

            httpContext.Response.Cookies.Append("AuthToken", token, cookieOptions);

            var userDto = new AuthResponseDto
            {
                Username = user.UserName,
            };

            return ServiceResult.Success(userDto);
        }

        public ServiceResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1),
            };

            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null) return ServiceResult.Failure(MessageDescriber.DefaultError());

            httpContext.Response.Cookies.Append("AuthToken", "", cookieOptions);

            var userDto = new AuthResponseDto();

            return ServiceResult.Success(userDto);
        }

        public async Task<ServiceResult> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return ServiceResult.Failure(MessageDescriber.UserNotFound());
            AuthResponseDto userDto = new()
            {
                Username = user.UserName
            };
            return ServiceResult.Success(userDto);
        }

        public async Task<RefreshToken> SetRefreshToken(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null) return new RefreshToken();

            var refreshToken = _tokenService.GenerateRefreshToken();
            appUser.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(appUser);
            return refreshToken;
        }
    }
}
