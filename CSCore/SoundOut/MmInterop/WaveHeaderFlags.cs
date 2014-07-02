using System;

namespace CSCore.SoundOut.MMInterop
{
    /// <summary>
    /// WaveHeaderFlags: http://msdn.microsoft.com/en-us/library/aa909814.aspx#1
    /// </summary>
    [Flags]
    public enum WaveHeaderFlags
    {
        WHDR_DONE = 0x00000001,
        WHDR_PREPARED = 0x00000002,
        WHDR_BEGINLOOP = 0x00000004,
        WHDR_ENDLOOP = 0x00000008,
        WHDR_INQUEUE = 0x00000010
    }
}