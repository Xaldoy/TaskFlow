using Service.DTOs.Error;

namespace TaskFlow.Service.DTOs.Error
{
    public class ErrorDescriber
    {
        public static ErrorResponse DefaultError() => new("Server error.", ErrorCode.DefaultError);
        public static ErrorResponse DefaultError(string errorMessage) => new(errorMessage, ErrorCode.DefaultError);
        public static ErrorResponse UserNotFound() => new("User not found.", ErrorCode.UserNotFound, ErrorTypes.AuthenticationError);
        public static ErrorResponse PasswordMismatch() => new("Wrong password.", ErrorCode.PasswordMismatch, ErrorTypes.AuthenticationError);
        public static ErrorResponse DuplicateUsername(string username) => new($"The username '{username}' is already taken.", ErrorCode.DuplicateUsername, ErrorTypes.AuthenticationError);
        public static ErrorResponse DuplicateEmail(string email) => new($"The email '{email}' is already taken.", ErrorCode.DuplicateEmail, ErrorTypes.AuthenticationError);
        public static ErrorResponse RegistrationError(string errorMessage) => new(errorMessage, ErrorCode.RegistrationError, ErrorTypes.AuthenticationError);
        public static ErrorResponse Unauthenticated() => new("Unauthenticated.", ErrorCode.Unauthenticated);
        public static ErrorResponse Unauthorized() => new("Unauthorized.", ErrorCode.Unauthorized);
        public static ErrorResponse InvalidModelState(string errorMessage, string errorType = ErrorTypes.GlobalError) => new(errorMessage, ErrorCode.InvalidModelState, errorType);
        public static ErrorResponse MethodNotAllowed() => new("Method not allowed.", ErrorCode.MethodNotAllowed);
        public static ErrorResponse AccountLockedOut(DateTimeOffset duration)
        {
            var time = duration - DateTimeOffset.UtcNow;
            return time.TotalSeconds <= 0
                ? new ErrorResponse("Your account is locked for a brief period due to multiple failed login attempts.", ErrorCode.AccountLockedOut, ErrorTypes.AuthenticationError)
                : new ErrorResponse($"Your account is locked for {string.Format("{0}:{1:00}s", (int)time.TotalMinutes, time.Seconds)} due to multiple failed login attempts.", ErrorCode.AccountLockedOut, ErrorTypes.AuthenticationError);
        }
    }

    public enum ErrorCode
    {
        DefaultError,
        UserNotFound,
        PasswordMismatch,
        DuplicateUsername,
        DuplicateEmail,
        RegistrationError,
        Unauthenticated,
        Unauthorized,
        InvalidModelState,
        MethodNotAllowed,
        AccountLockedOut
    }
}
