using TaskFlow.Model.Models;

namespace TaskFlow.DAL.Repositories.User
{
    public interface IUserRepository
    {
        public Task<FriendRelation> CreateFriendRelation(FriendRelation friendRelation);
        public Task<List<FriendRelation>> GetFriendRelations(string userId);
        public Task<FriendRelation?> GetFriendRelationForUsers(string user1Id, string user2Id);
        public Task<List<FriendRelation>> GetFriends(string userId);
        public Task<List<FriendRelation>> GetSentFriendRequests(string userId);
        public Task<List<FriendRelation>> GetReceivedFriendRequests(string userId);
        public Task DeleteFriendRequest(int friendRelationId);
        public Task<FriendRelation?> AcceptFriendRequest(int friendRelationId);
        public Task<FriendRelation?> DeclineFriendRequest(int friendRelationId);
    }
}
