using Microsoft.Extensions.Logging;
using NetSdrClient.Interfaces;
using NetSdrClient.Messages;
using NetSdrClient.Messages.Enums;

namespace NetSdrClient.Consumers;

public class TcpMessageConsumer(ILogger log) : IMessageConsumer
{
    public Task ConsumeAsync(byte[] message, CancellationToken cancellationToken = default)
    {
        if (MessageHelpers.IsNAK(message))
        {
            log.LogInformation($"Received NAK message!");
            return Task.CompletedTask;
        }

        var type = (TargetMessageType)MessageHelpers.GetMessageType(message);
        if (type == TargetMessageType.ACK)
        {
            var dataItemByte = message[3];
            log.LogInformation($"Received ACK message for data item {dataItemByte}!");
            return Task.CompletedTask;
        }

        ControlItemCode? controlItem = message.Length > 4 ? (ControlItemCode)MessageHelpers.GetControlItem(message) : null;

        if (type == TargetMessageType.UnsolicitedControlItem)
        {
            log.LogInformation($"Unsolicited message received for {controlItem?.ToString()}!");
            return Task.CompletedTask;
        }



        log.LogInformation($"Received message type: {type} with controlItem: {controlItem}, length {message.Length}");
        return Task.CompletedTask;
    }
}
