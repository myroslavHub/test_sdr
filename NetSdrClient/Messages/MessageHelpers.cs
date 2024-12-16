namespace NetSdrClient.Messages;

public static class MessageHelpers
{
    public static bool IsNAK(byte[] message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        
        return message.Length == 2 && message[0] == 0x02 && message[1] == 0x00;
    }


    public static int GetMessageLength(byte[] message)
    {
        if (message == null || message.Length < 2)
        {
            throw new ArgumentException("Message is not valid");
        }

        byte firstByte = message[0];  // LSB
        byte secondByte = message[1]; // 5 bits for MSB
        int length = firstByte | ((secondByte & 0b00011111) << 8);

        return length;
    }

    public static int GetMessageType(byte[] message)
    {
        if (message == null || message.Length < 2)
        {
            throw new ArgumentException("Message is not valid");
        }

        return (message[1] >> 5) & 0b00000111;
    }

    public static int GetControlItem(byte[] message)
    {
        if (message == null || message.Length < 4)
        {
            throw new ArgumentException("Message is not valid");
        }

        return message[3] << 8 | message[2];
    }
}
