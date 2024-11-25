using Confluent.Kafka;

namespace Share.Kafka;

public interface IKafkaConsumer<TValue>
{
    public void CreateConsumer(ConsumerConfig consumerConfig,CancellationToken cancellationToken = default);
    
    // rename
    public void ConsumerHandler(List<string> topics,Action<ConsumeResult<Ignore, TValue>>? callback, CancellationToken cancellationToken = default);
}