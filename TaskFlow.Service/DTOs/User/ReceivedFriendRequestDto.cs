namespace TaskFlow.Service.DTOs.User
{
    public class ReceivedFriendRequestDto : BaseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool? Accepted { get; set; }
    }
}
