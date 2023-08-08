using Dotnet.Telemetry.Jaeger.Api.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Telemetry.Jaeger.Api.Context
{
    public class AppDbContext : DbContext
    {
        public required DbSet<PersonEntity> Person { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
    }
}