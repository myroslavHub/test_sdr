using System.Net;

namespace NetSdrClient.Interfaces;

public interface ITcpClientWrapper
{
    bool IsConnected { get; }
    Task ConnectAsync(IPEndPoint endPoint, CancellationToken cancellationToken);
    Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
    Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
    void Close();
}
