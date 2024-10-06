using TaskFlow.Service.DTOs.Error;

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

                if (httpContext.Response.StatusCode == StatusCodes.Status405MethodNotAllowed)
                {
                    if (!httpContext.Response.HasStarted)
                    {
                        httpContext.Response.ContentType = "application/json";
                        await httpContext.Response.WriteAsJsonAsync(ErrorDescriber.MethodNotAllowed());
                    }
                }
            }
            catch (Exception ex)
            {
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsJsonAsync(ErrorDescriber.DefaultError(ex.Message));
                }
            }
        }
    }
}
