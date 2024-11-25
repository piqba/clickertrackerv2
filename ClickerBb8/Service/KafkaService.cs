using System.Text.Json;
using ClickerBb8.Database;
using Confluent.Kafka;
using Dapper;
using Microsoft.Extensions.Options;
using Share.dto;
using Share.Kafka;

namespace ClickerBb8.Service;

public class KafkaService(
    IOptions<KafkaOptions> opts,
    ILogger<KafkaService> logger,
    IDbConnectionFactory dbConnectionFactory,
    ConsumerFactory<WebPageEventDto> consumerFactory)
{
    public async Task RunAsync(CancellationToken stoppingToken)
    {
       
        var topics = opts.Value.TopicPrefix.Split(',').ToList();
        consumerFactory.CreateConsumer(new ConsumerConfig(opts.Value.KafkaConfig), stoppingToken);
        
        consumerFactory.ConsumerHandler(
            topics,
            async (consumerResult) =>
            {
                // var eventDto = JsonSerializer.Deserialize<WebPageEventDto>(consumerResult.Message.Value);
                var eventDto = consumerResult.Message.Value;

                // Insert on db
                using var dbConnection = await dbConnectionFactory.CreateConnectionAsync(stoppingToken);
                await dbConnection.ExecuteAsync(
                    """
                    insert into clicker_events_simple (element_id, element_type, element_text, page_url, page_title, path_name, id, country, locale, platform, user_agent,kafka_offset) 
                    values (@ElementId,@EventType,@ElementText,@PageUrl,@PageTitle, @PathName,@Id,@Country,@Locale,@Platform,@UserAgent,@KafkaOffset)
                    """,
                    new
                    {
                        eventDto.ElementId,
                        eventDto.EventType,
                        ElementText = eventDto.ElementType,
                        @eventDto.PageUrl,
                        @eventDto.PageTitle,
                        @eventDto.PathName,
                        @eventDto.Id,
                        @eventDto.Country,
                        @eventDto.Locale,
                        @eventDto.Platform,
                        @eventDto.UserAgent,
                        kafkaOffset = consumerResult.Offset.Value
                    }
                );
            }, stoppingToken);
    }
}