using Model.Models;
using TaskFlow.Model.Models;

namespace TaskFlow.DAL.Repositories.Authorization
{
    public interface IAuthorizationRepository
    {
        public Task<TaskCategory?> GetTaskCategory(int categoryId);
        public Task<TaskItem?> GetTaskItem(int taskItemId);
        public Task<FriendRelation?> GetFriendRelation(int friendRelationId);
    }
}
