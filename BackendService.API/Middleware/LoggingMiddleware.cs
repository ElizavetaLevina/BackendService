using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace BackendService.API.Middleware
{
    public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<LoggingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Items["StartTime"] = DateTime.UtcNow;

            var paramsString = string.Join(", ", httpContext.Request.Query.Select(p => $"{p.Key}={p.Value.ToString()}"));

            _logger.LogInformation("http.request : Before {Method} {Path}, params: {{{Params}}}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                string.IsNullOrEmpty(paramsString) ? "" : paramsString);

            await _next(httpContext);

            var startTime = (DateTime)httpContext.Items["StartTime"]!;
            var duration = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;

            if (httpContext.Response.StatusCode < 400)
            {
                _logger.LogInformation("http.request : After {Method} {Path}, status={StatusCode}, duration={Duration}ms",
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    httpContext.Response.StatusCode,
                    duration);
            }
            else if (httpContext.Response.StatusCode == 401)
            {
                _logger.LogWarning("http.request : UNAUTHORIZED {Method} {Path}, status=401, duration={Duration}ms",
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    duration);
            }
            else if (httpContext.Response.StatusCode == 403)
            {
                _logger.LogWarning("http.request : FORBIDDEN {Method} {Path}, status=403, duration={Duration}ms",
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    duration);
            }
        }
    }
}
