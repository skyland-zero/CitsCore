using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Cits.Api.Extensions;

public static class OpenTelemetryExtension
{
    public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services)
    {
        var otlpEndpoint = "http://192.168.10.14:4317";
        services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder =>
            {
                resourceBuilder
                    .AddService("CitsApi", "CitsApiNamespace", "0.0.1")
                    .AddTelemetrySdk();
            })
            .WithTracing(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint))
            )
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint))
            );

        //日志
        services.AddLogging(logging =>
        {
            logging.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint));
            });
        });

        return services;
    }
}
