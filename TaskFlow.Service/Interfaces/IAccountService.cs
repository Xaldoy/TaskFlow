using Model.Models;
using Service.DTOs;
using Service.DTOs.Auth;
using Service.DTOs.Result;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<UserDto>> LoginAsync(LoginAttemptDto loginAttempt);
        Task<ServiceResult<UserDto>> RegisterAsync(RegisterAttemptDto registerAttempt);
        Task<ServiceResult<UserDto>> GetUserByEmail(string email);
        Task<RefreshToken> SetRefreshToken(string userName);
        ServiceResult<string> RefreshToken(AppUser user, string refreshToken);
    }
}
