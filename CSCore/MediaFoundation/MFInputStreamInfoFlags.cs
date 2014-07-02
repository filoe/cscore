using System;

namespace CSCore.MediaFoundation
{
    [Flags]
    public enum MFInputStreamInfoFlags
    {
        WholeSamples = 0x00000001,
        SingleSamplePerBuffer = 0x00000002,
        FixedSampleSize = 0x00000004,
        HoldsBuffers = 0x00000008,
        DoesNotAddRef = 0x00000100,
        Removable = 0x00000200,
        Optional = 0x00000400,
        ProcessInPlace = 0x00000800
    }
}