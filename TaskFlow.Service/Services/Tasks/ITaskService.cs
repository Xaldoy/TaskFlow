using Service.DTOs.Result;
using TaskFlow.Service.DTOs.Task;

namespace TaskFlow.Service.Services.Tasks
{
    public interface ITaskService
    {
        Task<ServiceResult> GetTaskCategories();
        Task<ServiceResult> GetTasksForCategory(int categoryId);
        Task<ServiceResult> CreateTask(TaskItemDto taskItem);
        Task<ServiceResult> UpdateTask(TaskItemDto taskItem);
        Task<ServiceResult> DeleteTask(int taskItemId);
        Task<ServiceResult> CreateTaskCategory(TaskCategoryDto taskCategory);
        Task<ServiceResult> DeleteTaskCategory(int categoryId);
        Task<ServiceResult> GetTaskPriorities();
    }
}
