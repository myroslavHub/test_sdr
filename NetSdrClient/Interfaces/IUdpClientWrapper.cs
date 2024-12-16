using System.Net.Sockets;
using System.Net;

namespace NetSdrClient.Interfaces;

public interface IUdpClientWrapper
{
    void Connect(IPEndPoint endpoint);
    Task<UdpReceiveResult> ReceiveAsync(CancellationToken cancellationToken = default);
    void Close();
}
