using TaskFlow.Service.DTOs.Error;

namespace Service.DTOs.Error
{
    public record ErrorResponse(string ErrorMessage, ErrorCode ErrorCode, string ErrorType = ErrorTypes.GlobalError);
}
