using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.DTOs.Auth;
using Service.DTOs.Message;
using Service.DTOs.Result;
using System.Security.Claims;
using TaskFlow.Service.DTOs.Message;
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
            MessageResponse? modelStateErorr = GetModelStateError(MessageTypes.AuthenticationError);
            if (modelStateErorr != null)
                return MessageResult(modelStateErorr);

            ServiceResult serviceResult = await _authenticationService.LoginAsync(loginAttempt);
            return HandleServiceResult(serviceResult);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterAttemptDto registerAttempt)
        {
            MessageResponse? modelStateErorr = GetModelStateError(MessageTypes.AuthenticationError);
            if (modelStateErorr != null)
                return MessageResult(modelStateErorr);

            ServiceResult serviceResult = await _authenticationService.RegisterAsync(registerAttempt);
            return HandleServiceResult(serviceResult);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            ServiceResult serviceResult = _authenticationService.Logout();
            return HandleServiceResult(serviceResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            string? userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (userEmail == null) return MessageResult(MessageDescriber.Unauthenticated());

            ServiceResult serviceResult = await _authenticationService.GetUserByEmail(userEmail);
            return HandleServiceResult(serviceResult);
        }
    }
}
