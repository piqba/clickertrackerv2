using Confluent.Kafka;

namespace Share.Kafka;

public class ProducerFactory<TValue> : IKafkaProducer<TValue>
{
    private static IProducer<Null, TValue> _producer;

    public void CreateProducer(ProducerConfig producerConfig, CancellationToken cancellationToken = default)
    {
        _producer = new ProducerBuilder<Null, TValue>(producerConfig).Build();
    }

    public async Task ProduceAsync(string topic, TValue value, CancellationToken cancellationToken = default)
    {
        try
        {
            var dr = await _producer.ProduceAsync(topic, new Message<Null, TValue> { Value = value },
                cancellationToken);
            Console.WriteLine(
                $"Delivered offset: '{dr.Offset}' to '{dr.TopicPartitionOffset}' ts: '{dr.Timestamp.UnixTimestampMs}'");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Delivery failed: {e.Error.Reason}");
        }
    }
}