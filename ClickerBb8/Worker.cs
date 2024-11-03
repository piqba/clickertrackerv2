using System.Text.Json;
using ClickerBb8.Database;
using Confluent.Kafka;
using Dapper;
using Microsoft.Extensions.Options;
using Share;
using Share.dto;

namespace ClickerBb8;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOptions<KafkaOptions> _opts;
    private readonly IDBConnectionFactory _dbConnectionFactory;

    public Worker(ILogger<Worker> logger, IOptions<KafkaOptions> opts, IDBConnectionFactory dbConnectionFactory)
    {
        _logger = logger;
        _opts = opts;
        _dbConnectionFactory = dbConnectionFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig(_opts.Value.KafkaConfig))
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
                       .Build())

            {
                consumer.Subscribe(_opts.Value.TopicPrefix);

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(stoppingToken);

                            if (consumeResult.IsPartitionEOF)
                            {
                                Console.WriteLine(
                                    $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                                continue;
                            }

                            // add data on db
                            // Console.WriteLine(
                            //     $"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                            // deserialize
                            var eventDto = JsonSerializer.Deserialize<WebPageEventDto>(consumeResult.Message.Value);
                            // Insert on db
                            using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(stoppingToken);
                            await dbConnection.ExecuteAsync(
                                """
                                insert into clicker_events_simple (element_id, element_type, element_text, page_url, page_title, path_name, id, country, locale, platform, user_agent,kafka_offset) 
                                values (@ElementId,@EventType,@ElementText,@PageUrl,@PageTitle, @PathName,@Id,@Country,@Locale,@Platform,@UserAgent,@KafkaOffset)
                                """,
                                new
                                {
                                    eventDto.ElementId, eventDto.EventType, ElementText = eventDto.ElementType,
                                    @eventDto.PageUrl, @eventDto.PageTitle, @eventDto.PathName, @eventDto.Id,
                                    @eventDto.Country, @eventDto.Locale, @eventDto.Platform, @eventDto.UserAgent,
                                    kafkaOffset = consumeResult.Offset.Value
                                }
                            );

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
    }
}