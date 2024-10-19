using Microsoft.EntityFrameworkCore;
using Model;
using TaskFlow.Model.Models;

namespace TaskFlow.DAL.Repositories.User
{
    public class UserRepository(TaskFlowContext context) : IUserRepository
    {
        private readonly TaskFlowContext _context = context;

        public async Task<FriendRelation?> AcceptFriendRequest(int friendRelationId)
        {
            var friendRelation = await _context.FriendRelations.FindAsync(friendRelationId);
            if (friendRelation == null) return null;
            friendRelation.Accepted = true;
            await _context.SaveChangesAsync();
            return friendRelation;
        }

        public async Task<FriendRelation> CreateFriendRelation(FriendRelation friendRelation)
        {
            var createdFriendRelation = await _context.FriendRelations.AddAsync(friendRelation);
            await _context.SaveChangesAsync();
            return createdFriendRelation.Entity;
        }

        public async Task<FriendRelation?> DeclineFriendRequest(int friendRelationId)
        {
            var friendRelation = await _context.FriendRelations.FindAsync(friendRelationId);
            if (friendRelation == null) return null;
            friendRelation.Accepted = false;
            await _context.SaveChangesAsync();
            return friendRelation;
        }

        public async Task DeleteFriendRequest(int friendRelationId)
        {
            await _context.FriendRelations.Where(x => x.Id == friendRelationId).ExecuteDeleteAsync();
        }

        public async Task<FriendRelation?> GetFriendRelationForUsers(string user1Id, string user2Id)
        {
            var friendRelation = await _context.FriendRelations
                .FirstOrDefaultAsync(x => x.User1Id == user1Id && x.User2Id == user2Id);
            return friendRelation;
        }

        public async Task<List<FriendRelation>> GetFriendRelations(string userId)
        {
            var friendRelations = await _context.FriendRelations
                .Where(x => (x.Accepted == true && (x.User1Id == userId || x.User2Id == userId)) || (x.Accepted == null && x.User1Id == userId))
                .ToListAsync();
            return friendRelations;
        }

        public async Task<List<FriendRelation>> GetFriends(string userId)
        {
            var friends = await _context.FriendRelations
                .Include(x => x.User2)
                .Include(x => x.User1)
                .Where(x => x.Accepted == true && x.User1Id == userId || x.User2Id == userId)
                .ToListAsync();
            return friends;
        }

        public async Task<List<FriendRelation>> GetReceivedFriendRequests(string userId)
        {
            var receivedFriendRequests = await _context.FriendRelations
                .Include(x => x.User1)
                .Where(x => x.Accepted == null && x.User2Id == userId)
                .ToListAsync();
            return receivedFriendRequests;
        }

        public async Task<List<FriendRelation>> GetSentFriendRequests(string userId)
        {
            var sentFriendRequests = await _context.FriendRelations
               .Include(x => x.User2)
               .Where(x => x.Accepted == null && x.User1Id == userId)
               .ToListAsync();
            return sentFriendRequests;
        }
    }
}
