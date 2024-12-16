using NetSdrClient.Interfaces;
using System.Net.Sockets;
using NetSdrClient.Messages;
using NetSdrClient.Messages.Enums;
using Microsoft.Extensions.Logging;
using NetSdrClient.Wrapers;

namespace NetSdrClient;

public class NetSDRClient
{
    private readonly ConnectionConfig _connectionConfig;
    private readonly ITcpClientWrapper _tcpClient;
    private readonly IUdpClientWrapper _udpClient;
    private readonly ILogger _log;
    private readonly IMessageConsumer _tcpMessageConsumer;
    private readonly IMessageConsumer _udpMessageConsumer;

    private CancellationTokenSource _tcpReceiverCancellationTokenSource;
    private CancellationTokenSource _udpReceiverCancellationTokenSource;

    public NetSDRClient(ConnectionConfig connectionConfig, ILogger logger,
        IMessageConsumer tcpMessageConsumer, IMessageConsumer udpMessageConsumer,
        ITcpClientWrapper? tcpClient = default, IUdpClientWrapper? udpClient = default)
    {
        _tcpClient = tcpClient ?? new TcpClientWrapper();
        _udpClient = udpClient ?? new UdpClientWrapper();
        _log = logger;
        _tcpMessageConsumer = tcpMessageConsumer;
        _udpMessageConsumer = udpMessageConsumer;
        _connectionConfig = connectionConfig;
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        _log.LogInformation("Connecting to NetSDR....");
        await _tcpClient.ConnectAsync(_connectionConfig.TcpEndpoint, cancellationToken);


        _tcpReceiverCancellationTokenSource = new CancellationTokenSource();
        _ = Task.Run(ReceiveTcpMessagesAsync).ContinueWith(task =>
        {
            if (task.IsFaulted && task.Exception != null)
            {
                _log.LogError($"Failed during receiving tcp messages {task.Exception}...");
            }
        });

        _log.LogInformation("Connected to NetSDR.");
    }

    public void Disconnect()
    {
        _log.LogInformation("Disconnecting from NetSDR.");
        _udpReceiverCancellationTokenSource?.Cancel();
        _tcpReceiverCancellationTokenSource?.Cancel();
        _tcpClient.Close();
        _udpClient.Close();
        _log.LogInformation("Disconnected from NetSDR.");
    }

    public async Task RequestNameAsync(CancellationToken cancellationToken = default)
    {
        _log.LogInformation("Starting requesting device name...");

        // Send command to start receiving data
        byte[] startMessage = MessageBuilder.GetDeviceName();
        await _tcpClient.WriteAsync(startMessage, 0, startMessage.Length, cancellationToken);
    }

    public async Task StartReceivingAsync(CancellationToken cancellationToken = default)
    {

        _log.LogInformation("Starting receiving of Real 16-bit FIFO data...");


        byte[] startMessage = MessageBuilder.StartReceiver(ReceiverStateDataType.RealADSample, ReceiverStateCaptureMode.FIFO16bit, 1);
        _log.LogInformation("Sending ReceiverState message to start");
        await _tcpClient.WriteAsync(startMessage, 0, startMessage.Length, cancellationToken);

        _udpReceiverCancellationTokenSource = new CancellationTokenSource();

        _ = Task.Run(ReceiveUdpPacketsAsync, cancellationToken).ContinueWith(task =>
        {
            if (task.IsFaulted && task.Exception != null)
            {
                _log.LogError($"Failed during receiving udp messages {task.Exception}...");
            }
        });
    }

    public async Task SetFrequencyAsync(long frequency, CancellationToken cancellationToken = default)
    {
        _log.LogInformation($"Setting frequency to {frequency} Hz...");

        byte[] message = MessageBuilder.ChangeFrequency(frequency);
        await _tcpClient.WriteAsync(message, 0, message.Length, cancellationToken);

        _log.LogInformation("Frequency set.");
    }

    public async Task StopReceivingAsync(CancellationToken cancellationToken = default)
    {
        _log.LogInformation("Stopping receiving of Real 16-bit FIFO data...");

        // Send command to stop receiving data
        byte[] stopMessage = MessageBuilder.StopReceiver();
        await _tcpClient.WriteAsync(stopMessage, 0, stopMessage.Length, cancellationToken);

        await _udpReceiverCancellationTokenSource.CancelAsync();
    }

    private async Task ReceiveTcpMessagesAsync()
    {
        // this may be not enaph
        byte[] buffer = new byte[1024];

        while (!_tcpReceiverCancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                int bytesRead = await _tcpClient.ReadAsync(buffer, 0, buffer.Length, _tcpReceiverCancellationTokenSource.Token);
                if (bytesRead > 0)
                {
                    byte[] message = new byte[bytesRead];
                    Array.Copy(buffer, message, bytesRead);
                    
                    // not working in C#12 ?????
                    // var msg = new ReadOnlySpan<byte>(buffer, 0, bytesRead);
                    
                    await _tcpMessageConsumer.ConsumeAsync(message, _tcpReceiverCancellationTokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation($"Error receiving TCP message: {ex.Message}");
            }
        }
    }

    private async Task ReceiveUdpPacketsAsync()
    {
       _udpClient.Connect(_connectionConfig.UdpEndpoint);
        while (!_udpReceiverCancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                UdpReceiveResult result = await _udpClient.ReceiveAsync(_udpReceiverCancellationTokenSource.Token);
                
                await _udpMessageConsumer.ConsumeAsync(result.Buffer, _udpReceiverCancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                _log.LogInformation($"Error receiving UDP packet: {ex.Message}");
            }
        }
        _udpClient.Close();
    }
}
