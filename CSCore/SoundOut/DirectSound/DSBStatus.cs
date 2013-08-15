using System;

namespace CSCore.SoundOut.DirectSound
{
    [Flags]
    public enum DSBStatus
    {
        Playing = 0x00000001,
        BufferLost = 0x00000002,
        Looping = 0x00000004,
        LocHardware = 0x00000008,
        LocSoftware = 0x00000010,
        Terminated = 0x00000020
    }
}