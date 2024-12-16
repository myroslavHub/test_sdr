namespace NetSdrClient.Messages.Enums;

public enum HostMessageType : byte
{
    SetControlItem = 0b000,
    RequestCurrentControlItem = 0b001,
    RequestControlItemRange = 0b010,
    ACK = 0b011,
    DataItem0 = 0b100,
    DataItem1 = 0b101,
    DataItem2 = 0b110,
    DataItem3 = 0b111,
}

public enum TargetMessageType : byte
{
    SetRequestCurrentControlItemResponse = 0b000,
    UnsolicitedControlItem = 0b001,
    RequestControlItemRangeResponse = 0b010,
    ACK = 0b011,
    DataItem0 = 0b100,
    DataItem1 = 0b101,
    DataItem2 = 0b110,
    DataItem3 = 0b111,
}
