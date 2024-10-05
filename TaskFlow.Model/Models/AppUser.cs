using Microsoft.AspNetCore.Identity;

namespace Model.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<TaskCategory> TaskCategories { get; set; } = new List<TaskCategory>();
    }
}
