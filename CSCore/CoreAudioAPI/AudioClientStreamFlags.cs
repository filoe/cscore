using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// For details see:
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd370791(v=vs.85).aspx and
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd370789(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum AudioClientStreamFlags
    {
        None = 0x0,
        StreamFlagsCrossProcess = 0x00010000,
        StreamFlagsLoopback = 0x00020000,
        StreamFlagsEventCallback = 0x00040000,
        StreamFlagsNoPersist = 0x00080000,

        /// <summary>
        /// Supported since Windows 7
        /// </summary>
        StreamFlagsRateAdjust = 0x00100000,

        SessionFlagsExpireWhenUnowned = 0x10000000,
        SessionFlagsDisplayHide = 0x20000000,
        SessionFlagsDisplayHideWhenExpired = 0x40000000
    }
}
