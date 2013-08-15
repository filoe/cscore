using System;

namespace CSCore.Tags.ID3
{
    [Flags]
    public enum ID3v2ExtendedHeaderFlags
    {
        None = 0x0,
        TagUpdate = 0x4,
        CrcPresent = 0x2,
        Restrict = 0x1
    }
}