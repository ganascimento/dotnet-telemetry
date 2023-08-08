using Dotnet.Telemetry.Jaeger.Api.Context;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Telemetry.Jaeger.Api.Configuration;

public static class ConfigureContext
{
    public static void ConfigContext(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("testdb"));
    }
}