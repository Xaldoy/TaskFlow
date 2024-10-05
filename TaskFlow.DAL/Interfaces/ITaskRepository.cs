using Model.Models;

namespace DAL.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetTasksByCategory(int categoryId);
        Task<List<TaskCategory>> GetTaskCategories(string userId);
        Task<TaskItem> CreateTask(TaskItem taskItem);
        Task<TaskItem> UpdateTask(TaskItem taskItem);
        Task DeleteTask(int taskItemId);
        Task<TaskCategory?> GetCategoryById(int categoryId);
        Task<TaskItem?> GetTaskById(int taskId);
        Task<TaskCategory> CreateTaskCategory(TaskCategory category);
        Task DeleteTaskCategory(int categoryId);
        Task<List<TaskPriority>> GetTaskPriorities();
    }
}
