using FluentAssertions;
using NetSdrClient.Messages;
using NetSdrClient.Messages.Enums;

namespace NetSdrClient.Tests;

public class MessageBuilderTests
{
    [Test]
    public void VerifyStopMessage()
    {
        byte[] expected = [0x08, 0x00, 0x18, 0x00, 0x00, 0x01, 0x00, 0x00];
        var actual = MessageBuilder.StopReceiver();

        Assert.That(actual.SequenceEqual(expected), Is.True);
    }

    [Test]
    public void VerifyStartComplexContiguous24BitModeMessage()
    {
        byte[] expected = [0x08, 0x00, 0x18, 0x00, 0x80, 0x02, 0x80, 0x00];

        var actual = MessageBuilder.StartReceiver(ReceiverStateDataType.Complex, ReceiverStateCaptureMode.Continuos24bit);

        Assert.That(actual.SequenceEqual(expected), Is.True);
    }

    [Test]
    public void VerifyStartRealFifo16bit()
    {
        byte[] expected = [0x08, 0x00, 0x18, 0x00, 0x00, 0x02, 0x01, 0x01];

        var actual = MessageBuilder.StartReceiver(ReceiverStateDataType.RealADSample, ReceiverStateCaptureMode.FIFO16bit, 1);

        Assert.That(actual.SequenceEqual(expected), Is.True);
    }

    [Test]
    public void VerifyChangeFrequencyMessage()
    {
        const long frequency = 100_000_000;

        byte[] result = MessageBuilder.ChangeFrequency(frequency);

        byte[] frequencyBytes = BitConverter.GetBytes(frequency).Take(5).ToArray();
        result.Skip(5).Take(5).Should().Equal(frequencyBytes);
    }


    [Test]
    public void GetName()
    {
        byte[] expected = [4, 0x20, 1, 0];
        var actual = MessageBuilder.GetDeviceName();

        Assert.That(actual.SequenceEqual(expected), Is.True);
    }
}
