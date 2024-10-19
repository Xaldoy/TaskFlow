using TaskFlow.Service.DTOs.Message;

namespace Service.DTOs.Message
{
    public record MessageResponse(string Message, MessageCode MessageCode, MessageType MessageType);
}
