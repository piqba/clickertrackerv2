using System.Text.Json;
using System.Threading.Channels;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Share;
using Share.dto;

namespace ClickerC3p0.Clicks;

public static class Endpoint
{
    public static WebApplication MapClickEndpoint(this WebApplication app)
    {
        app.MapPost("/clicks", async (WebPageEventDto webPageEventDto, KafkaService svc) =>
        {
            var jsonString = JsonSerializer.Serialize(webPageEventDto);
            await svc.SendClicksEventsAsync(jsonString);
            return Results.Json(null, statusCode: StatusCodes.Status202Accepted);
        });

        app.MapGet("/clicks-stream", async (HttpContext ctx, KafkaService svc, CancellationToken ct) =>
        {
            ctx.Response.Headers.Append("Content-Type", "text/event-stream");
            await svc.ConsumeClickEvents(ctx,ct);
        });
        
        return app;
    }
}