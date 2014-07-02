using System;

namespace CSCore.MediaFoundation
{
    [Flags]
    public enum MFOutputStreamInfoFlags
    {
        WholeSamples = 0x1,
        SingleSamplePerBuffer = 0x2,
        FixedSampleSize = 0x4,
        Discardable = 0x8,
        Optional = 0x10,
        ProvidesSamples = 0x100,
        CanProvideSamples = 0x200,
        LazyRead = 0x400,
        Removable = 0x800
    }
}