using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Error;
using Service.DTOs.Result;
using Service.Utility;

namespace API.Controllers
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult ErrorResult(ErrorResponse errorResponse)
        {
            switch (errorResponse.ErrorCode)
            {
                #region Client Errors (4xx)

                #region 400 Bad Request
                case ErrorCode.RegistrationError:
                case ErrorCode.InvalidModelState:
                    return StatusCode(400, errorResponse);
                #endregion

                #region 401 Unauthorized
                case ErrorCode.PasswordMismatch:
                case ErrorCode.Unauthenticated:
                    return StatusCode(401, errorResponse);
                #endregion

                #region 403 Forbidden
                case ErrorCode.AccountLockedOut:
                case ErrorCode.Unauthorized:
                    return StatusCode(403, errorResponse);
                #endregion

                #region 404 Not Found
                case ErrorCode.UserNotFound:
                    return StatusCode(404, errorResponse);
                #endregion

                #region 405 Method Not Allowed
                case ErrorCode.MethodNotAllowed:
                    return StatusCode(405, errorResponse);
                #endregion

                #endregion

                #region Conflict Errors (409)

                #region 409 Conflict
                case ErrorCode.DuplicateUsername:
                case ErrorCode.DuplicateEmail:
                    return StatusCode(409, errorResponse);
                #endregion

                #endregion

                #region Server Errors (5xx)

                #region 500 Internal Server Error
                case ErrorCode.DefaultError:
                default:
                    return StatusCode(500, errorResponse);
                    #endregion

                    #endregion
            }

        }

        [NonAction]
        public IActionResult HandleServiceResult<T>(ServiceResult<T> serviceResult)
        {
            if (serviceResult.Error != null) return ErrorResult(serviceResult.Error);
            return Ok(serviceResult.Data);
        }

        [NonAction]
        public IActionResult HandleServiceResult(ServiceResult serviceResult)
        {
            if (serviceResult.Error != null) return ErrorResult(serviceResult.Error);
            return ErrorResult(ErrorDescriber.DefaultError());
        }

        [NonAction]
        public ErrorResponse? GetModelStateError()
        {
            if (!ModelState.IsValid && ModelState.Values.Any())
            {
                var modelState = ModelState.Values.First();
                var modelStateError = modelState.Errors.FirstOrDefault();
                if (modelStateError != null)
                {
                    return ErrorDescriber.InvalidModelState(modelStateError.ErrorMessage);
                }
            }
            return null;
        }
    }
}
