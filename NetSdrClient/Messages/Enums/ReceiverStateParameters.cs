namespace NetSdrClient.Messages.Enums;

public enum ReceiverStateDataType : byte
{
    RealADSample = 0b00000000,
    Complex = 0b10000000,
}

public enum ReceiverStateCommand : byte
{
    IdleStop = 0x01,
    Run = 0x02,
}

public enum ReceiverStateCaptureMode : byte
{
    Continuos16bit = 0x00,
    Continuos24bit = 0x80,
    FIFO16bit = 0x01,
    Pulse24bit = 0x83,
    Pulse16bit = 0x03
}