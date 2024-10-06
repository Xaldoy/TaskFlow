using Model.Models;
using Service.DTOs.Auth;
using Service.DTOs.Result;
using TaskFlow.Service.DTOs.Auth;

namespace TaskFlow.Service.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginAttemptDto loginAttempt);
        Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterAttemptDto registerAttempt);
        Task<ServiceResult<AuthResponseDto>> GetUserByEmail(string email);
        Task<RefreshToken> SetRefreshToken(string userName);
        ServiceResult<string> RefreshToken(AppUser user, string? refreshToken);
    }
}
