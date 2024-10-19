using TaskFlow.Service.DTOs.Message;

namespace API.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == StatusCodes.Status405MethodNotAllowed && !httpContext.Response.HasStarted)
                {
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsJsonAsync(MessageDescriber.MethodNotAllowed());
                }
            }
            catch (UnauthorizedAccessException)
            {
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsJsonAsync(MessageDescriber.Unauthorized());
                }
            }
            catch (Exception ex)
            {
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsJsonAsync(MessageDescriber.DefaultError(ex.Message));
                }
            }
        }
    }
}
