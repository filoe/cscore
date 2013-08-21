using System;

namespace CSCore.ACM
{
    [Flags]
    public enum AcmConvertFlags
    {
        ACM_STREAMCONVERTF_START = 0x00000010,
        ACM_STREAMCONVERTF_END = 0x00000020,
        ACM_STREAMCONVERTF_BLOCKALIGN = 0x00000004
    }
}