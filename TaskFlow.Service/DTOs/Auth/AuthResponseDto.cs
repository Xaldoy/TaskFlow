using System.Text.Json.Serialization;

namespace TaskFlow.Service.DTOs.Auth
{
    public class AuthResponseDto : BaseDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Username { get; set; } = string.Empty;
    }
}
