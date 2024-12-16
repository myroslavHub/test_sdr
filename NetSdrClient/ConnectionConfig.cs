using System.Net;

namespace NetSdrClient;

public record ConnectionConfig(IPEndPoint TcpEndpoint, IPEndPoint UdpEndpoint)
{
    public static ConnectionConfig Default()
    {
        var tcpEndpoint = new IPEndPoint(IPAddress.Loopback, 50000);
        var udpEndpoint = new IPEndPoint(IPAddress.Loopback, 60000);
        return new ConnectionConfig(tcpEndpoint, udpEndpoint);

    }
}
