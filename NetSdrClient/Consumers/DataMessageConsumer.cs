using Microsoft.Extensions.Logging;
using NetSdrClient.Interfaces;
using System.IO;

namespace NetSdrClient.Consumers;

/// <summary>
/// Writes received udp package to the file
/// </summary>
public class DataMessageConsumer : IMessageConsumer, IDisposable
{
    private readonly ILogger _log;
    private readonly FileStream _fileStream;

    public DataMessageConsumer(string filename, ILogger log)
    {
        _log = log;
        _fileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
    }

    public async Task ConsumeAsync(byte[] message, CancellationToken cancellationToken = default)
    {
        _log.LogInformation($"Received UDP packet of size {message.Length} bytes. Writing to file...");

        var offset = 4;

        await _fileStream.WriteAsync(message, offset, message.Length - offset, cancellationToken);
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
    }
}
