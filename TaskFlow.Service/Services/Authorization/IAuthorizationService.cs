using Model.Models;
using TaskFlow.Model.Models;

namespace TaskFlow.Service.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> UserOwnsTaskCategory(int taskCategoryId);
        bool UserOwnsTaskCategory(TaskCategory taskCategory);

        Task<bool> UserOwnsTask(int taskItemId);
        bool UserOwnsTask(TaskItem taskItem);

        Task<bool> UserOwnsSentFriendRelation(int friendRelationId);
        bool UserOwnsSentFriendRelation(FriendRelation friendRelation);

        Task<bool> UserOwnsReceivedFriendRelation(int friendRelationId);
        bool UserOwnsReceivedFriendRelation(FriendRelation friendRelation);

        string GetUserId();
    }
}
