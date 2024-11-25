using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Share.dto;
using Share.Kafka;
using Share.Otel;

namespace ClickerC3p0.Clicks;

public class KafkaService(
    IOptions<KafkaOptions> opts,
    ProducerFactory<WebPageEventDto> producerFactory,
    ConsumerFactory<WebPageEventDto> consumerFactory)
{
    public async Task SendClicksEventsAsync(WebPageEventDto jsonString)
    {
        using var activity = ClicksMetricsCustoms.ClicksTrackerActivitySource.StartActivity(Constants.ClicksActivity);
        var cfg = opts.Value;
        producerFactory.CreateProducer(new ProducerConfig(opts.Value.KafkaConfig));
        await producerFactory.ProduceAsync(opts.Value.TopicPrefix, jsonString);
        activity?.SetTag($"{Constants.ClicksTagKeyPrefix}.SendClicksEventsAsync", "SendClicksEventsAsync service");

    }

    public async Task ConsumeClickEvents(HttpContext ctx, CancellationToken ct = default)
    {
        consumerFactory.CreateConsumer(new ConsumerConfig(opts.Value.KafkaConfig), ct);
        var topics = opts.Value.TopicPrefix.Split(',').ToList();
        consumerFactory.ConsumerHandler(
            topics,
            async (consumeResult) =>
            {
                // Create a new channel for strings
                await ctx.Response.WriteAsync($"data: ", cancellationToken: ct);
                await JsonSerializer.SerializeAsync(ctx.Response.Body, consumeResult.Message,
                    cancellationToken: ct);
                await ctx.Response.WriteAsync($"\n\n", cancellationToken: ct);
                await ctx.Response.Body.FlushAsync(ct);
            },
            ct
        );
    }
}