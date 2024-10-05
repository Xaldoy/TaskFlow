using Service.DTOs.Result;

namespace Service.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> UserOwnsTaskCategory(int categoryId);
        Task<bool> UserOwnsTask(int taskItemId);
        ServiceResult<string> GetUserId();
    }
}
