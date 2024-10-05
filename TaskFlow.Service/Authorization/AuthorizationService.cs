using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Service.DTOs.Result;
using Service.Utility;
using System.Security.Claims;

namespace Service.Authorization
{
    public class AuthorizationService(IHttpContextAccessor httpContextAccessor, IAuthorizationRepository authorizationRepository) : IAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;

        public async Task<bool> UserOwnsTaskCategory(int categoryId)
        {
            ServiceResult<string> serviceResult = GetUserId();
            var category = await _authorizationRepository.GetTaskCategory(categoryId);
            return (category != null && serviceResult.Data == category.OwnerId);
        }

        public async Task<bool> UserOwnsTask(int taskItemId)
        {
            ServiceResult<string> serviceResult = GetUserId();
            var task = await _authorizationRepository.GetTaskItem(taskItemId);
            return (task != null && serviceResult.Data == task.TaskCategory.OwnerId);
        }

        public ServiceResult<string> GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return ServiceResult<string>.Failure(ErrorDescriber.Unauthenticated());
            }

            return ServiceResult<string>.Success(userIdClaim.Value);
        }

    }
}
