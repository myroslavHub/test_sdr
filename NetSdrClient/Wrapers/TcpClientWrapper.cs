using NetSdrClient.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace NetSdrClient.Wrapers;

public class TcpClientWrapper : ITcpClientWrapper
{
    private TcpClient _tcpClient;
    private NetworkStream? _stream;

    public bool IsConnected => _tcpClient != null && _tcpClient.Connected && _stream != null;

    public TcpClientWrapper() { }

    public async Task ConnectAsync(IPEndPoint endPoint, CancellationToken cancellationToken = default)
    {
        if (IsConnected)
        {
            return;
        }
        _tcpClient = new TcpClient();
        await _tcpClient.ConnectAsync(endPoint, cancellationToken);
        _stream = _tcpClient.GetStream();
    }

    public async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        if (_stream == null)
        {
            throw new InvalidOperationException("Not connected.");
        }
        await _stream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        if (_stream == null)
        {
            throw new InvalidOperationException("Not connected.");
        }
        return await _stream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public void Close()
    {
        _stream?.Close();
        _tcpClient.Close();
        _stream = null;
    }
}
