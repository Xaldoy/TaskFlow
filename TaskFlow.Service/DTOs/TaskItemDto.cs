namespace Service.DTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        public int TaskCategoryId { get; set; }
        public string TaskPriorityName { get; set; } = string.Empty;
    }
}
