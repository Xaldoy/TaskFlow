using System.Text.Json.Serialization;

namespace TaskFlow.Service.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string? UserName { get; set; } = string.Empty;
    }
}
