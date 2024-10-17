using System.Text.Json.Serialization;

namespace TaskFlow.Service.DTOs.Auth
{
    public class AuthResponseDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? UserName { get; set; } = string.Empty;
    }
}
