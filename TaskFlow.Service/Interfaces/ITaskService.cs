using Service.DTOs;
using Service.DTOs.Result;

namespace Service.Interfaces
{
    public interface ITaskService
    {
        Task<ServiceResult<List<TaskCategoryDto>>> GetTaskCategories();
        Task<ServiceResult<List<TaskItemDto>>> GetTasksForCategory(int categoryId);
        Task<ServiceResult<TaskItemDto>> CreateTask(TaskItemDto taskItem);
        Task<ServiceResult<TaskItemDto>> UpdateTask(TaskItemDto taskItem);
        Task<ServiceResult> DeleteTask(int taskItemId);
        Task<ServiceResult<TaskCategoryDto>> CreateTaskCategory(TaskCategoryDto taskCategory);
        Task<ServiceResult> DeleteTaskCategory(int categoryId);
        Task<ServiceResult<List<TaskPriorityDto>>> GetTaskPriorities();
    }
}
