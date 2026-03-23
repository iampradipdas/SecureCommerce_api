using SecureCommerce_api.Middleware;

namespace SecureCommerce_api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UsePerformanceLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<PerformanceLoggingMiddleware>();
    }
}
