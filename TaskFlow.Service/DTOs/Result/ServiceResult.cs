using Service.DTOs.Error;
using System.Text.Json.Serialization;
using TaskFlow.Service.DTOs.Error;
using TaskFlow.Service.DTOs.Error.Constants;

namespace Service.DTOs.Result
{
    public class ServiceResult
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MessageResponse? Message { get; set; }

        [JsonIgnore]
        public bool IsError => Message != null && Message.MessageType.Severity == MessageSeverities.Error;
        public static ServiceResult Success() => new() { };
        public static ServiceResult Failure(MessageResponse? message = null) => new() { Message = message ?? MessageDescriber.DefaultError() };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data) => new() { Data = data };
        new public static ServiceResult<T> Failure(MessageResponse? message = null) => new() { Message = message ?? MessageDescriber.DefaultError() };
    }
}