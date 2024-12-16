using FluentAssertions;
using Microsoft.Extensions.Logging;
using NetSdrClient.Consumers;
using NSubstitute;

namespace NetSdrClient.Tests;

public class DataConsumerTests
{
    private ILogger _mockLogger;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = Substitute.For<ILogger>();
    }

    [Test]
    public async Task DataMessageConsumer_ShouldWriteToFile()
    {
        string tempFile = Path.GetTempFileName();
        byte[] udpMessage = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 }; // Example message

        using (var consumer = new DataMessageConsumer(tempFile, _mockLogger))
        {
            await consumer.ConsumeAsync(udpMessage);
        }

        byte[] fileContents = await File.ReadAllBytesAsync(tempFile);
        fileContents.Should().Equal(udpMessage.Skip(4).ToArray());

        File.Delete(tempFile);
    }
}
