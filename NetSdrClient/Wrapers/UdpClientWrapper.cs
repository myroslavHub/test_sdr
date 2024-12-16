using System.Net.Sockets;
using System.Net;
using NetSdrClient.Interfaces;

namespace NetSdrClient.Wrapers;

public class UdpClientWrapper : IUdpClientWrapper
{
    private UdpClient _udpClient;

    public UdpClientWrapper() { }

    public void Connect(IPEndPoint endpoint)
    {
        _udpClient = new UdpClient();
        _udpClient.Client.Bind(endpoint);
    }

    public async Task<UdpReceiveResult> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        return await _udpClient.ReceiveAsync(cancellationToken);
    }

    public void Close()
    {
        _udpClient?.Close();
    }
}
