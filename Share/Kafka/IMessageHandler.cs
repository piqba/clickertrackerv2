namespace Share.Kafka;

public interface IMessageHandler<in TValue>
{
    Task HandlerMessageAsync(TValue message, CancellationToken cancellationToken = default);
}