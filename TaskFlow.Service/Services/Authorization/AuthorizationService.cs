using Microsoft.AspNetCore.Http;
using Model.Models;
using Service.DTOs.Result;
using System.Security.Claims;
using TaskFlow.DAL.Repositories.Authorization;
using TaskFlow.Service.DTOs.Error;

namespace TaskFlow.Service.Services.Authorization
{
    public class AuthorizationService(IHttpContextAccessor httpContextAccessor, IAuthorizationRepository authorizationRepository) : IAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;

        public async Task<bool> UserOwnsTaskCategory(int taskCategoryId)
        {
            ServiceResult<string> serviceResult = GetUserId();
            var category = await _authorizationRepository.GetTaskCategory(taskCategoryId);
            return category != null && serviceResult.Data == category.OwnerId;
        }

        public bool UserOwnsTaskCategory(TaskCategory taskCategory)
        {
            ServiceResult<string> serviceResult = GetUserId();
            return serviceResult.Data == taskCategory.OwnerId;
        }

        public async Task<bool> UserOwnsTask(int taskItemId)
        {
            ServiceResult<string> serviceResult = GetUserId();
            var task = await _authorizationRepository.GetTaskItem(taskItemId);
            return task != null && serviceResult.Data == task.TaskCategory.OwnerId;
        }

        public bool UserOwnsTask(TaskItem taskItem)
        {
            ServiceResult<string> serviceResult = GetUserId();
            return serviceResult.Data == taskItem.TaskCategory.OwnerId;
        }

        public ServiceResult<string> GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return ServiceResult<string>.Failure(MessageDescriber.Unauthenticated());
            }

            return ServiceResult<string>.Success(userIdClaim.Value);
        }

    }
}
