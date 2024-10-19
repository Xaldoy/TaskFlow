namespace TaskFlow.Service.DTOs.Task
{
    public class TaskPriorityDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
