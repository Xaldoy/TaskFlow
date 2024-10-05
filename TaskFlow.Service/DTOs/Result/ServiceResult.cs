using Service.DTOs.Error;
using Service.Utility;
using System.Text.Json.Serialization;

namespace Service.DTOs.Result
{
    public class ServiceResult
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ErrorResponse? Error { get; set; }

        [JsonIgnore]
        public bool IsError => Error != null;

        public static ServiceResult Success() => new() { };
        public static ServiceResult Failure(ErrorResponse? error = null) => new() { Error = error ?? ErrorDescriber.DefaultError() };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data) => new() { Data = data };
        new public static ServiceResult<T> Failure(ErrorResponse? error = null) => new() { Error = error ?? ErrorDescriber.DefaultError() };
    }
}