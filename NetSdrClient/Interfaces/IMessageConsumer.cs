namespace NetSdrClient.Interfaces;

public interface IMessageConsumer
{
    Task ConsumeAsync(byte[] message, CancellationToken cancellationToken = default);
}
