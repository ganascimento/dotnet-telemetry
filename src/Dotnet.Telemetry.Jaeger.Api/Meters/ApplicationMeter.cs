using System.Diagnostics.Metrics;

namespace Dotnet.Telemetry.Jaeger.Api.Meters
{
    public static class ApplicationMeter
    {
        private static readonly Meter meter = new Meter("Application.Metrics");
        public static readonly Counter<int> totalRequests = meter.CreateCounter<int>("compute_requests");
        public static readonly Counter<int> totalGets = meter.CreateCounter<int>("compute_gets");
        public static readonly Counter<int> totalPosts = meter.CreateCounter<int>("compute_posts");
    }
}