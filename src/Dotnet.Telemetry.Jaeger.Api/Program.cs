using Dotnet.Telemetry.Jaeger.Api.Configuration;
using Dotnet.Telemetry.Jaeger.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigOpenTelemetry(builder.Configuration, builder.Host);
builder.Services.ConfigContext();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<MetricsMiddleware>();

app.Run();
