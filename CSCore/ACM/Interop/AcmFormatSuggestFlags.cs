using System;

namespace CSCore.ACM
{
    [Flags]
    public enum AcmFormatSuggestFlags
    {
        FormatTag = 0x00010000,
        Channels = 0x00020000,
        SamplesPerSecond = 0x00040000,
        BitsPerSample = 0x00080000,
        TypeMask = 0x00FF0000,
    }
}