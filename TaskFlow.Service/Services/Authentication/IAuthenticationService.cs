using Model.Models;
using Service.DTOs.Auth;
using Service.DTOs.Result;

namespace TaskFlow.Service.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResult> LoginAsync(LoginAttemptDto loginAttempt);
        Task<ServiceResult> RegisterAsync(RegisterAttemptDto registerAttempt);
        ServiceResult Logout();
        Task<ServiceResult> GetUserByEmail(string email);
        Task<RefreshToken> SetRefreshToken(string userName);
    }
}
