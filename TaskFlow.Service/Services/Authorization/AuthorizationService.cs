using Microsoft.AspNetCore.Http;
using Model.Models;
using System.IdentityModel.Tokens.Jwt;
using TaskFlow.DAL.Repositories.Authorization;
using TaskFlow.Model.Models;

namespace TaskFlow.Service.Services.Authorization
{
    public class AuthorizationService(IHttpContextAccessor httpContextAccessor, IAuthorizationRepository authorizationRepository) : IAuthorizationService
    {
        // This service should remain self-contained and must not inject any other services 
        // to prevent potential circular dependencies.
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
        private string UserId => GetUserId();

        public async Task<bool> UserOwnsTaskCategory(int taskCategoryId)
        {
            var category = await _authorizationRepository.GetTaskCategory(taskCategoryId);
            return category != null && UserId == category.OwnerId;
        }

        public bool UserOwnsTaskCategory(TaskCategory taskCategory)
        {
            return UserId == taskCategory.OwnerId;
        }

        public async Task<bool> UserOwnsTask(int taskItemId)
        {
            var task = await _authorizationRepository.GetTaskItem(taskItemId);
            return task != null && UserId == task.TaskCategory.OwnerId;
        }

        public bool UserOwnsTask(TaskItem taskItem)
        {
            return UserId == taskItem.TaskCategory.OwnerId;
        }

        public string GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("No HTTP context available.");
            var token = httpContext.Request.Cookies["AuthToken"];

            if (token == null) throw new UnauthorizedAccessException();

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwt = jwtHandler.ReadJwtToken(token);
            var claim = jwt.Claims.FirstOrDefault(claim => claim.Type == "nameid");
            return claim == null ? throw new UnauthorizedAccessException() : claim.Value;
        }

        public bool UserOwnsSentFriendRelation(FriendRelation friendRelation)
        {
            return UserId == friendRelation.User1Id;
        }

        public bool UserOwnsReceivedFriendRelation(FriendRelation friendRelation)
        {
            return UserId == friendRelation.User2Id;
        }

        public async Task<bool> UserOwnsSentFriendRelation(int friendRelationId)
        {
            var friendRelation = await _authorizationRepository.GetFriendRelation(friendRelationId);
            return friendRelation != null && friendRelation.User1Id == UserId;
        }

        public async Task<bool> UserOwnsReceivedFriendRelation(int friendRelationId)
        {
            var friendRelation = await _authorizationRepository.GetFriendRelation(friendRelationId);
            return friendRelation != null && friendRelation.User2Id == UserId;
        }
    }
}
