﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Service.DTOs.Auth;
using Service.DTOs.Result;
using TaskFlow.Service.DTOs.Auth;
using TaskFlow.Service.DTOs.Error;

namespace TaskFlow.Service.Services.Authentication
{
    public class AuthenticationService(UserManager<AppUser> userManager, TokenService tokenService, SignInManager<AppUser> signInManager) : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly TokenService _tokenService = tokenService;
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginAttemptDto loginAttempt)
        {
            var user = await _userManager.FindByEmailAsync(loginAttempt.Credentials);
            user ??= await _userManager.FindByNameAsync(loginAttempt.Credentials);

            if (user == null)
            {
                return ServiceResult<AuthResponseDto>.Failure(ErrorDescriber.UserNotFound());
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginAttempt.Password, true);

            if (result.IsLockedOut)
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                if (!lockoutEnd.HasValue)
                {
                    return ServiceResult<AuthResponseDto>.Failure(ErrorDescriber.DefaultError());
                }
                return ServiceResult<AuthResponseDto>.Failure(ErrorDescriber.AccountLockedOut(lockoutEnd.Value));
            }

            if (!result.Succeeded) return ServiceResult<AuthResponseDto>.Failure(ErrorDescriber.PasswordMismatch());

            var userDto = new AuthResponseDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return ServiceResult<AuthResponseDto>.Success(userDto);
        }

        public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterAttemptDto registerAttempt)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerAttempt.UserName))
                return ServiceResult<AuthResponseDto>.Failure(ErrorDescriber.DuplicateUsername(registerAttempt.UserName));

            if (await _userManager.Users.AnyAsync(x => x.Email == registerAttempt.Email))
                return ServiceResult<AuthResponseDto>.Failure(ErrorDescriber.DuplicateEmail(registerAttempt.Email));

            var user = new AppUser
            {
                Email = registerAttempt.Email,
                UserName = registerAttempt.UserName,
            };
            var result = await _userManager.CreateAsync(user, registerAttempt.Password);

            if (!result.Succeeded && result.Errors.Any())
            {
                string errorMessage = result.Errors.First().Description;
                return ServiceResult<AuthResponseDto>.Failure(ErrorDescriber.RegistrationError(errorMessage));
            }

            var userDto = new AuthResponseDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return ServiceResult<AuthResponseDto>.Success(userDto);
        }

        public async Task<ServiceResult<AuthResponseDto>> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return ServiceResult<AuthResponseDto>.Failure(ErrorDescriber.UserNotFound());
            AuthResponseDto userDto = new()
            {
                UserName = user.UserName
            };
            return ServiceResult<AuthResponseDto>.Success(userDto);
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

        public ServiceResult<string> RefreshToken(AppUser user, string? refreshToken)
        {
            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
            if (oldToken != null && !oldToken.IsActive)
                return ServiceResult<string>.Failure(ErrorDescriber.Unauthenticated());

            if (oldToken != null) oldToken.Revoked = DateTime.UtcNow;
            var newToken = _tokenService.CreateToken(user);
            if (newToken == null)
                return ServiceResult<string>.Failure(ErrorDescriber.Unauthenticated());

            return ServiceResult<string>.Success(newToken);
        }
    }
}
