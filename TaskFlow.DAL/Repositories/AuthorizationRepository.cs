using DAL.Interfaces;
using Model;
using Model.Models;

namespace Repository
{
    public class AuthorizationRepository(TaskFlowContext context) : IAuthorizationRepository
    {
        private readonly TaskFlowContext _context = context;

        public async Task<TaskCategory?> GetTaskCategory(int categoryId)
        {
            return await _context.TaskCategories.FindAsync(categoryId);
        }

        public async Task<TaskItem?> GetTaskItem(int taskItemId)
        {
            return await _context.TaskItems.FindAsync(taskItemId);
        }
    }
}
