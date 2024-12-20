﻿namespace NetSdrClient.Messages.Enums;

public enum ControlItemCode : ushort
{
    None = 0,
    TargetName = 0x0001,
    TargetSerialNumber = 0x0002,
    InterfaceVersion = 0x0003,
    HardwareFirmwareVersions = 0x0004,
    StatusErrorCode = 0x0005,
    ProductID = 0x0009,
    Options = 0x000A,
    SecurityCode = 0x000B,
    FPGAConfiguration = 0x000C,
    ReceiverState = 0x0018,
    ReceiverChannelSetup = 0x0019,
    ReceiverFrequency = 0x0020,
    ReceiverNCOPhaseOffset = 0x0022,
    ReceiverADAmplitudeScale = 0x0023,
    RFGain = 0x0038,
    VHFUHFDownConverterGain = 0x003A,
    RFFilterSelection = 0x0044,
    ADModes = 0x008A,
    IQOutputDataSampleRate = 0x00B8,
    ADInputSampleRateCalibration = 0x00B0,
    DCOffsetCalibration = 0x00D0,
    InputSyncModes = 0x00B4,
    InternalTriggerFrequency = 0x00B2,
    InternalTriggerPhaseOffset = 0x00B3,
    PulseOutputModes = 0x00B6,
    DataOutputPacketSize = 0x00C4,
    DataOutputUDPIPAndPortAddress = 0x00C5,
    CWStartupMessage = 0x0150,
    RS232SerialPortOpen = 0x0200,
    RS232SerialPortClose = 0x0201
}