namespace Model.Models
{
    public class TaskPriority
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<TaskItem> TaskItems { get; set; } = null!;
    }
}
