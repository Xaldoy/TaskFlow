using Microsoft.AspNetCore.Identity;
using TaskFlow.Model.Models;

namespace Model.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<TaskCategory> TaskCategories { get; set; } = new List<TaskCategory>();
        public ICollection<FriendRelation> FriendRelationsAsUser1 { get; set; } = new List<FriendRelation>();
        public ICollection<FriendRelation> FriendRelationsAsUser2 { get; set; } = new List<FriendRelation>();
    }
}
