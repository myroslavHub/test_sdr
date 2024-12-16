using NetSdrClient.Messages.Enums;

namespace NetSdrClient.Messages;

public class MessageBuilder
{
    public static byte[] ChangeFrequency(long frequency)
    {
        byte[] frequencyBytes = BitConverter.GetBytes(frequency).Take(5).ToArray();
        
        return BuildMessage(ControlItemCode.ReceiverFrequency, HostMessageType.SetControlItem, [(byte)Channel.Channel1, ..frequencyBytes]);
    }

    public static byte[] StopReceiver()
    {
        return BuildMessage(ControlItemCode.ReceiverState, HostMessageType.SetControlItem, 0, (byte)ReceiverStateCommand.IdleStop, 0, 0);
    }

    public static byte[] StartReceiver(ReceiverStateDataType dataType, ReceiverStateCaptureMode captureMode, byte numberOfSamples = 0)
    {
        return BuildMessage(ControlItemCode.ReceiverState, HostMessageType.SetControlItem, (byte)dataType, (byte)ReceiverStateCommand.Run, (byte)captureMode, numberOfSamples);
    }

    public static byte[] GetDeviceName()
    {
        return BuildMessage(ControlItemCode.TargetName, HostMessageType.RequestCurrentControlItem);
    }

    private static byte[] BuildMessage(ControlItemCode controlItemCode, HostMessageType messageType, params byte[] parameters)
    {
        int length = 4 + parameters.Length; // Header + Control Item Code + Parameters
        byte[] message = new byte[length];

        // Header (16 bits)
        message[0] = (byte)(length & 0xFF); // Length LSB
        message[1] = (byte)(length >> 8 & 0x1F | (byte)messageType << 5); // Type + Length MSB


        // Control Item Code (16 bits)
        message[2] = (byte)((ushort)controlItemCode & 0xFF); // LSB
        message[3] = (byte)((ushort)controlItemCode >> 8 & 0xFF); // MSB

        // Parameters
        Array.Copy(parameters, 0, message, 4, parameters.Length);

        return message;
    }
}