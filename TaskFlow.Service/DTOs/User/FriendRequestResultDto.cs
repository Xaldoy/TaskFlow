using System.Text.Json.Serialization;

namespace TaskFlow.Service.DTOs.User
{
    public class FriendRequestResultDto : BaseDto
    {
        public bool Success { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }
    }
}
