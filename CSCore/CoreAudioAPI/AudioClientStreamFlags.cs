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
        StreamFlags_CrossProcess = 0x00010000,
        StreamFlags_Loopback = 0x00020000,
        StreamFlags_EventCallback = 0x00040000,
        StreamFlags_NoPersist = 0x00080000,

        /// <summary>
        /// Supported since Windows 7
        /// </summary>
        StreamFlags_RateAdjust = 0x00100000,

        SessionFlags_ExpireWhenUnowned = 0x10000000,
        SessionFlags_DisplayHide = 0x20000000,
        SessionFlags_Display_HideWhenExpired = 0x40000000
    }
}
