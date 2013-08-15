using System;

namespace CSCore.SoundOut.DirectSound
{
    [Flags]
    public enum DSCapabilitiesFlags
    {
        Certified = 0x40,
        ContinousRate = 0x10,
        EmulatedDriver = 0x20,
        None = 0,
        PrimaryBuffer16Bit = 8,
        PrimaryBuffer8Bit = 4,
        PrimaryBufferMono = 1,
        PrimaryBufferStereo = 2,
        SecondaryBuffer16Bit = 0x800,
        SecondaryBuffer8Bit = 0x400,
        SecondaryBufferMono = 0x100,
        SecondaryBufferStereo = 0x200
    }
}