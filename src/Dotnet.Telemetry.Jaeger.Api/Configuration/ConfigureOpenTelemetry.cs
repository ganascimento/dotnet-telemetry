using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Dotnet.Telemetry.Jaeger.Api.Configuration;

public static class ConfigureOpenTelemetry
{
    public static void ConfigOpenTelemetry(this IServiceCollection services, IConfiguration configuration, IHostBuilder hostBuilder)
    {
        services.AddOpenTelemetry()
            .WithTracing(options =>
            {
                options
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Dotnet.Telemetry.Jaeger.Tracing"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()

                    .SetSampler(new AlwaysOnSampler())
                    .AddJaegerExporter(jaegerOptions =>
                    {
                        jaegerOptions.AgentHost = configuration["Jaeger:Url"] ?? throw new InvalidOperationException();
                        jaegerOptions.AgentPort = int.Parse(configuration["Jaeger:Port"] ?? throw new InvalidOperationException());
                    })
                    .AddOtlpExporter(o => o.Endpoint = new Uri(configuration["Otlp:Endpoint"] ?? throw new InvalidOperationException()));
            })
            .WithMetrics(options =>
            {
                options
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Dotnet.Telemetry.Jaeger.Metrics"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter("Application.Metrics")
                    .AddOtlpExporter(o => o.Endpoint = new Uri(configuration["Otlp:Endpoint"] ?? throw new InvalidOperationException()));
            });

        services.AddLogging(logging => logging.AddOpenTelemetry(openTelemetryLoggerOptions =>
        {
            openTelemetryLoggerOptions.IncludeScopes = true;
            openTelemetryLoggerOptions.IncludeFormattedMessage = true;
            openTelemetryLoggerOptions.ParseStateValues = true;

            openTelemetryLoggerOptions.AddOtlpExporter(exporter =>
            {
                exporter.Endpoint = new Uri(configuration["Otlp:Endpoint"] ?? throw new InvalidOperationException());
            });
        })
        .SetMinimumLevel(LogLevel.Debug));
    }
}