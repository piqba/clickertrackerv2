using Confluent.Kafka;
using Share.Otel;

namespace Share.Kafka;

public class ProducerFactory<TValue> : IKafkaProducer<TValue>
{
    private static IProducer<Null, TValue> _producer;

    public void CreateProducer(ProducerConfig producerConfig, CancellationToken cancellationToken = default)
    {
        _producer = new ProducerBuilder<Null, TValue>(producerConfig)
            .SetValueSerializer(new JsonValueSerializer<TValue>())
            .Build();
    }

    public async Task ProduceAsync(string topic, TValue value, CancellationToken cancellationToken = default)
    {

        try
        {
            using var activity = ClicksMetricsCustoms.ClicksTrackerActivitySource.StartActivity(Constants.ClicksActivity);

            var dr = await _producer.ProduceAsync(topic, new Message<Null, TValue> { Value = value },
                cancellationToken);
            // Console.WriteLine(
            //     $"Delivered offset: '{dr.Offset}' to '{dr.TopicPartitionOffset}' ts: '{dr.Timestamp.UnixTimestampMs}'");
            activity?.SetTag($"{Constants.ClicksTagKeyPrefix}.ProduceAsync",new
            {
                dr.Offset,
                Timestamp = dr.Timestamp.UnixTimestampMs,
                dr.Topic,
            });

        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}