using System.Diagnostics.Metrics;
using Dotnet.Telemetry.Jaeger.Api.Meters;

namespace Dotnet.Telemetry.Jaeger.Api.Middleware;

public class MetricsMiddleware
{
    private readonly RequestDelegate _next;

    public MetricsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        ApplicationMeter.totalRequests.Add(1);

        if (httpContext.Request.Method == "GET") ApplicationMeter.totalGets.Add(1);
        if (httpContext.Request.Method == "POST") ApplicationMeter.totalPosts.Add(1);

        await _next(httpContext);
    }
}