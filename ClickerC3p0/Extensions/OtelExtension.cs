using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Share.Otel;

namespace ClickerC3p0.Extensions;

public static class OtelExtension
{
    public static IServiceCollection AddOtel(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OtelOptions>(configuration.GetSection(Constants.OtelKeyPropertie));
        var opts =configuration.GetSection(Constants.OtelKeyPropertie).Get<OtelOptions>();
        var otelEndpointUrl = opts?.OtelEndpointUrl ?? null;
        var otelApplicationName = opts?.OtelApplicationName ?? Constants.DefaultOtelApplicationName;
        var otel = services.AddOpenTelemetry();

        // Configure OpenTelemetry Resources with the application name
        otel.ConfigureResource(resource => resource
            .AddService(serviceName: otelApplicationName));

        // Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
        otel.WithMetrics(metrics => metrics
            // Metrics provider from OpenTelemetry
            .AddAspNetCoreInstrumentation()
            .AddMeter(ClicksMetricsCustoms.ClickCounterMeter.Name)
            // Metrics provides by ASP.NET Core in .NET 8
            .AddMeter("Microsoft.AspNetCore.Hosting")
            .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
            .AddPrometheusExporter());

        // Add Tracing for ASP.NET Core and our custom ActivitySource and export to Jaeger
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddSource(ClicksMetricsCustoms.ClicksTrackerActivitySource.Name);
            
            if (otelEndpointUrl != null)
            {
                tracing.AddOtlpExporter(otlpOptions => { otlpOptions.Endpoint = new Uri(otelEndpointUrl); });
                // tracing.AddConsoleExporter();

            }
            else
            {
                tracing.AddConsoleExporter();
            }
        });
        return services;
    }
}