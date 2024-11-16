namespace Share.IKafkaService;

public interface IMessageHandler<in TValue>
{
    Task HandlerMessageAsync(TValue message, CancellationToken cancellationToken = default);
}