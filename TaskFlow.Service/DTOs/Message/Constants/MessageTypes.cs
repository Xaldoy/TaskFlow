using TaskFlow.Service.DTOs.Message.Constants;

namespace TaskFlow.Service.DTOs.Message
{
    public class MessageTypes
    {
        public static MessageType AuthenticationError = new("authentication_error", MessageSeverities.Error);
        public static MessageType GlobalError = new("global_error", MessageSeverities.Error);
        public static MessageType BadRequest = new("request_error", MessageSeverities.Error);
    }
}
