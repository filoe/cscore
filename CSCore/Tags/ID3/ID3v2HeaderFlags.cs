using System;

namespace CSCore.Tags.ID3
{
    [Flags]
    public enum ID3v2HeaderFlags
    {
        Unsynchronisation = 0x80,
        ExtendedHeader = 0x40,
        ExperimentalIndicator = 0x20,
        FooterPresent = 0x10
    }
}