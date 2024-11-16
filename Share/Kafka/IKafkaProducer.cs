using Confluent.Kafka;

namespace Share.Kafka;

public interface IKafkaProducer<TValue>
{
    public void CreateProducer(ProducerConfig producerConfig,CancellationToken cancellationToken = default);
    public Task ProduceAsync(string topic, TValue value,CancellationToken cancellationToken = default);
}   