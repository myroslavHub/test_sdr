using Microsoft.Extensions.Logging;
using NetSdrClient.Interfaces;
using NetSdrClient.Messages;
using NetSdrClient.Messages.Enums;
using NSubstitute;
using System.Net;

namespace NetSdrClient.Tests;

public class NetSDRClientTests
{
    private ITcpClientWrapper _mockTcpClient;
    private IUdpClientWrapper _mockUdpClient;
    private ILogger _mockLogger;
    private IMessageConsumer _mockTcpConsumer;
    private IMessageConsumer _mockUdpConsumer;

    private NetSDRClient _client;

    [SetUp]
    public void SetUp()
    {
        _mockTcpClient = Substitute.For<ITcpClientWrapper>();
        _mockUdpClient = Substitute.For<IUdpClientWrapper>();
        _mockLogger = Substitute.For<ILogger>();
        _mockTcpConsumer = Substitute.For<IMessageConsumer>();
        _mockUdpConsumer = Substitute.For<IMessageConsumer>();

        var config = ConnectionConfig.Default();
        _client = new NetSDRClient(config, _mockLogger, _mockTcpConsumer, _mockUdpConsumer, _mockTcpClient, _mockUdpClient);
    }

    [Test]
    public async Task ConnectAsync_ShouldEstablishTcpConnection()
    {
        // Act
        await _client.ConnectAsync();

        // Assert
        await _mockTcpClient.Received(1).ConnectAsync(Arg.Any<IPEndPoint>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task SetFrequencyAsync_ShouldSendCorrectMessage()
    {
        // Arrange
        const long frequency = 100_000_000;
        byte[] expectedMessage = MessageBuilder.ChangeFrequency(frequency);

        // Act
        await _client.SetFrequencyAsync(frequency);

        // Assert
        await _mockTcpClient.Received(1).WriteAsync(
            Arg.Is<byte[]>(buffer => buffer.SequenceEqual(expectedMessage)),
            0,
            expectedMessage.Length,
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task StartReceivingAsync_ShouldSendCorrectMessage()
    {
        // Arrange
        byte[] expectedMessage = MessageBuilder.StartReceiver(ReceiverStateDataType.RealADSample, ReceiverStateCaptureMode.FIFO16bit, 1);

        // Act
        await _client.StartReceivingAsync();

        // Assert
        await _mockTcpClient.Received(1).WriteAsync(
            Arg.Is<byte[]>(buffer => buffer.SequenceEqual(expectedMessage)),
            0,
            expectedMessage.Length,
            Arg.Any<CancellationToken>());

    }

    [Test]
    public async Task StopReceivingAsync_ShouldSendStopReceiverMessage()
    {
        // Arrange
        byte[] expectedMessage = MessageBuilder.StopReceiver();

        // Act
        await _client.StartReceivingAsync();
        await _client.StopReceivingAsync();

        // Assert
        await _mockTcpClient.Received(1).WriteAsync(
            Arg.Is<byte[]>(buffer => buffer.SequenceEqual(expectedMessage)),
            0,
            expectedMessage.Length,
            Arg.Any<CancellationToken>());

    }
}
