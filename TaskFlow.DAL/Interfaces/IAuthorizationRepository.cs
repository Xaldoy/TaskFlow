using Model.Models;

namespace DAL.Interfaces
{
    public interface IAuthorizationRepository
    {
        public Task<TaskCategory?> GetTaskCategory(int categoryId);
        public Task<TaskItem?> GetTaskItem(int taskItemId);
    }
}
