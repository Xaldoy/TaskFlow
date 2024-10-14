using TaskFlow.Service.DTOs.Error;

namespace Service.DTOs.Error
{
    public record MessageResponse(string Message, MessageCode MessageCode, MessageType MessageType);
}
