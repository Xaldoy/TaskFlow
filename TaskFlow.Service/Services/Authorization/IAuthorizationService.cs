using Model.Models;
using Service.DTOs.Result;

namespace TaskFlow.Service.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> UserOwnsTaskCategory(int taskCategoryId);
        bool UserOwnsTaskCategory(TaskCategory taskCategory);
        Task<bool> UserOwnsTask(int taskItemId);
        bool UserOwnsTask(TaskItem taskItem);
        ServiceResult<string> GetUserId();
    }
}
