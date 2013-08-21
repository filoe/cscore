using System;

namespace CSCore.ACM
{
    [Flags]
    public enum AcmHeaderSate
    {
        ACMSTREAMHEADER_STATUSF_DONE = 0x00010000,
        ACMSTREAMHEADER_STATUSF_INQUEUE = 0x00100000,
        ACMSTREAMHEADER_STATUSF_PREPARED = 0x00020000
    }
}