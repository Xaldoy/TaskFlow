using Service.DTOs.Message;
using System.Text.Json.Serialization;
using TaskFlow.Service.DTOs;
using TaskFlow.Service.DTOs.Message;
using TaskFlow.Service.DTOs.Message.Constants;

namespace Service.DTOs.Result
{
    public class ServiceResult
    {
        public object? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MessageResponse? Message { get; set; }

        [JsonIgnore]
        public bool IsError => Message != null && Message.MessageType.Severity == MessageSeverities.Error;

        public static ServiceResult Success(object data) => new() { Data = data };
        public static ServiceResult Failure(MessageResponse? message = null) => new() { Message = message ?? MessageDescriber.DefaultError() };
        public static ServiceResult Success() => new();
        public static ServiceResult Success(IEnumerable<BaseDto> data) => new() { Data = data };
    }
}