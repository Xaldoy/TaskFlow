using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Error;
using Service.DTOs.Result;
using TaskFlow.Service.DTOs.Error;

namespace API.Controllers
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult MessageResult(MessageResponse messageResponse)
        {
            switch (messageResponse.MessageCode)
            {
                #region Client Errors (4xx)

                #region 400 Bad Request
                case MessageCode.RegistrationError:
                case MessageCode.InvalidModelState:
                    return StatusCode(400, messageResponse);
                #endregion

                #region 401 Unauthorized
                case MessageCode.PasswordMismatch:
                case MessageCode.Unauthenticated:
                    return StatusCode(401, messageResponse);
                #endregion

                #region 403 Forbidden
                case MessageCode.AccountLockedOut:
                case MessageCode.Unauthorized:
                    return StatusCode(403, messageResponse);
                #endregion

                #region 404 Not Found
                case MessageCode.UserNotFound:
                    return StatusCode(404, messageResponse);
                #endregion

                #region 405 Method Not Allowed
                case MessageCode.MethodNotAllowed:
                    return StatusCode(405, messageResponse);
                #endregion

                #endregion

                #region Conflict Errors (409)

                #region 409 Conflict
                case MessageCode.DuplicateUsername:
                case MessageCode.DuplicateEmail:
                    return StatusCode(409, messageResponse);
                #endregion

                #endregion

                #region Server Errors (5xx)

                #region 500 Internal Server Error
                case MessageCode.DefaultError:
                default:
                    return StatusCode(500, messageResponse);
                    #endregion

                    #endregion
            }

        }

        [NonAction]
        public IActionResult HandleServiceResult<T>(ServiceResult<T> serviceResult)
        {
            if (serviceResult.Message != null && serviceResult.IsError) return MessageResult(serviceResult.Message);
            return Ok(serviceResult.Data);
        }

        [NonAction]
        public IActionResult HandleServiceResult(ServiceResult serviceResult)
        {
            if (serviceResult.Message != null && serviceResult.IsError) return MessageResult(serviceResult.Message);
            return MessageResult(MessageDescriber.DefaultError());
        }

        [NonAction]
        public MessageResponse? GetModelStateError(MessageType? messageType = null )
        {
            if (!ModelState.IsValid && ModelState.Values.Any())
            {
                var modelState = ModelState.Values.First();
                var modelStateError = modelState.Errors.FirstOrDefault();
                if (modelStateError != null)
                {
                    return MessageDescriber.InvalidModelState(modelStateError.ErrorMessage, messageType ?? MessageTypes.GlobalError);
                }
            }
            return null;
        }
    }
}
