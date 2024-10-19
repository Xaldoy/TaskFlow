using Model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Model.Models
{
    public class FriendRelation
    {
        public int Id { get; set; }
        public string User1Id { get; set; } = string.Empty;
        public string User2Id { get; set; } = string.Empty;
        public bool? Accepted { get; set; } = null;
        [ForeignKey("User1Id")]
        public AppUser User1 { get; set; } = null!;
        [ForeignKey("User2Id")]
        public AppUser User2 { get; set; } = null!;
    }
}
