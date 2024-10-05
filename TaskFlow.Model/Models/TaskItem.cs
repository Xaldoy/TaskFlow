using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Model.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? DateTimeStart { get; set; } = null!;
        public DateTime? DateTimeEnd { get; set; } = null!;
        [AllowNull]
        public int? TaskPriorityId { get; set; } = null;
        public int TaskCategoryId { get; set; }
        [ForeignKey("TaskCategoryId")]
        public TaskCategory TaskCategory { get; set; } = null!;
        [ForeignKey("TaskPriorityId")]
        public TaskPriority? TaskPriority { get; set; } = null;
    }
}
