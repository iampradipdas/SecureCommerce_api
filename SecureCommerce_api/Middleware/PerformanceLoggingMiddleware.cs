using System.Diagnostics;
using System.Security.Claims;

namespace SecureCommerce_api.Middleware;

public class PerformanceLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceLoggingMiddleware> _logger;

    public PerformanceLoggingMiddleware(RequestDelegate next, ILogger<PerformanceLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Let the request proceed to the next middleware/controller
        await _next(context);

        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        var request = context.Request;
        var response = context.Response;
        
        // Extract user identity from claims if available
        var user = context.User.Identity?.IsAuthenticated == true 
            ? context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "Authenticated User"
            : "Anonymous";

        // Practical logging for e-commerce performance monitoring
        if (elapsedMilliseconds > 500) // Log slow requests as warnings
        {
            _logger.LogWarning(
                "SLOW REQUEST: {Method} {Path} by {User} responded {StatusCode} in {Elapsed}ms",
                request.Method,
                request.Path,
                user,
                response.StatusCode,
                elapsedMilliseconds
            );
        }
        else
        {
            _logger.LogInformation(
                "REQUEST: {Method} {Path} by {User} responded {StatusCode} in {Elapsed}ms",
                request.Method,
                request.Path,
                user,
                response.StatusCode,
                elapsedMilliseconds
            );
        }
    }
}
