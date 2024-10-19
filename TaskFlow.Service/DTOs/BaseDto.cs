using Service.DTOs.Message;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace TaskFlow.Service.DTOs
{
    public class BaseDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [SwaggerIgnore]
        public MessageResponse? MessageResponse { get; set; } = null;
    }
}
