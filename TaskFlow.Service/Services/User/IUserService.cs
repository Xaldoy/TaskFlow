using Service.DTOs.Result;

namespace TaskFlow.Service.Services.User
{
    public interface IUserService
    {
        public Task<ServiceResult> SendFriendRequest(string username);
        public Task<ServiceResult> GetReceivedFriendRequests();
        public Task<ServiceResult> GetSentFriendRequests();
        public Task<ServiceResult> GetFriends();
        public Task<ServiceResult> DeleteFriendRequest(int friendRelationId);
        public Task<ServiceResult> AcceptFriendRequest(int friendRelationId);
        public Task<ServiceResult> DeclineFriendRequest(int friendRelationId);
    }
}
