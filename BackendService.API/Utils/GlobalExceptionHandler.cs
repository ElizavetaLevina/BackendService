using BackendService.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.API.Utils
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var startTime = httpContext.Items.ContainsKey("StartTime") ? (DateTime)httpContext.Items["StartTime"]! : DateTime.UtcNow;
            var duration = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            if (exception is BaseException e)
            {
                httpContext.Response.StatusCode = (int)e.StatusCode;
                problemDetails.Title = e.Message;
            }
            else
            {
                problemDetails.Title = exception.Message;
            }

            _logger.LogError(exception, "http.request : FAILED {Method} {Path}, status={StatusCode}, duration={Duration}ms, error={Error}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Response.StatusCode,
                duration,
                string.IsNullOrEmpty(problemDetails.Title) ? "Internal server error" : problemDetails.Title);

            problemDetails.Status = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
