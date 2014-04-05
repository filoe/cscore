using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore
{
    /// <summary>
    /// Channelmask for WaveFormatExtensible. For more infos see
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd757714(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum ChannelMask
    {
        SpeakerFrontLeft = 0x1,
        SpeakerFrontRight = 0x2,
        SpeakerFrontCenter = 0x4,
        SpeakerLowFrequency = 0x8,
        SpeakerBackLeft = 0x10,
        SpeakerBackRight = 0x20,
        SpeakerFrontLeftOfCenter = 0x40,
        SpeakerFrontRightOfCenter = 0x80,
        SpeakerBackCenter = 0x100,
        SpeakerSideLeft = 0x200,
        SpeakerSideRight = 0x400,
        SpeakerTopCenter = 0x800,
        SpeakerTopFrontLeft = 0x1000,
        SpeakerTopFrontCenter = 0x2000,
        SpeakerTopFrontRight = 0x4000,
        SpeakerTopBackLeft = 0x8000,
        SpeakerTopBackCenter = 0x10000,
        SpeakerTopBackRight = 0x20000
    }
}
