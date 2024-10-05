using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Models
{
    public class TaskCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        [ForeignKey("OwnerId")]
        public AppUser Owner { get; set; } = null!;
        public ICollection<TaskItem> TaskItems { get; set; } = null!;
    }
}
