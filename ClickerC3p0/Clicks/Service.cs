using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Share;

namespace ClickerC3p0.Clicks;

public class KafkaService(IOptions<KafkaOptions> opts)
{
    public async Task SendClicksEventsAsync(string jsonString )
    {
        var cfg = opts.Value;
        using var p = new ProducerBuilder<Null, string>(new ProducerConfig(opts.Value.KafkaConfig)).Build();
        try
        {
            var dr = await p.ProduceAsync(opts.Value.TopicPrefix,new Message<Null, string> { Value = jsonString });
            Console.WriteLine(
                $"Delivered offset: '{dr.Offset}' to '{dr.TopicPartitionOffset}' ts: '{dr.Timestamp.UnixTimestampMs}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }

    public async Task ConsumeClickEvents(HttpContext ctx ,CancellationToken ct = default)
    {
        
            using var consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig(opts.Value.KafkaConfig))
                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    // Since a cooperative assignor (CooperativeSticky) has been configured, the
                    // partition assignment is incremental (adds partitions to any existing assignment).
                    Console.WriteLine(
                        "Partitions incrementally assigned: [" +
                        string.Join(',', partitions.Select(p => p.Partition.Value)) +
                        "], all: [" +
                        string.Join(',', c.Assignment.Concat(partitions).Select(p => p.Partition.Value)) +
                        "]");

                    // Possibly manually specify start offsets by returning a list of topic/partition/offsets
                    // to assign to, e.g.:
                    // return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    // Since a cooperative assignor (CooperativeSticky) has been configured, the revoked
                    // assignment is incremental (may remove only some partitions of the current assignment).
                    var remaining = c.Assignment.Where(atp =>
                        partitions.Where(rtp => rtp.TopicPartition == atp).Count() == 0);
                    Console.WriteLine(
                        "Partitions incrementally revoked: [" +
                        string.Join(',', partitions.Select(p => p.Partition.Value)) +
                        "], remaining: [" +
                        string.Join(',', remaining.Select(p => p.Partition.Value)) +
                        "]");
                })
                .SetPartitionsLostHandler((c, partitions) =>
                {
                    // The lost partitions handler is called when the consumer detects that it has lost ownership
                    // of its assignment (fallen out of the group).
                    Console.WriteLine($"Partitions were lost: [{string.Join(", ", partitions)}]");
                })
                .Build();
            consumer.Subscribe(opts.Value.TopicPrefix);

            try
            {
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(ct);

                        if (consumeResult.IsPartitionEOF)
                        {
                            Console.WriteLine(
                                $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                            continue;
                        }

                        Console.WriteLine(
                            $"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                        await ctx.Response.WriteAsync($"data: ", cancellationToken: ct);
                        await JsonSerializer.SerializeAsync(ctx.Response.Body, consumeResult.Message.Value, cancellationToken: ct);
                        await ctx.Response.WriteAsync($"\n\n", cancellationToken: ct);
                        await ctx.Response.Body.FlushAsync(ct);
                        try
                        {
                            // Store the offset associated with consumeResult to a local cache. Stored offsets are committed to Kafka by a background thread every AutoCommitIntervalMs. 
                            // The offset stored is actually the offset of the consumeResult + 1 since by convention, committed offsets specify the next message to consume. 
                            // If EnableAutoOffsetStore had been set to the default value true, the .NET client would automatically store offsets immediately prior to delivering messages to the application. 
                            // Explicitly storing offsets after processing gives at-least once semantics, the default behavior does not.
                            consumer.StoreOffset(consumeResult);
                        }
                        catch (KafkaException e)
                        {
                            Console.WriteLine($"Store Offset error: {e.Error.Reason}");
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Closing consumer.");
                consumer.Close();
            }
    }
}