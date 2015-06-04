using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="EndpointHardwareSupportFlags"/> are hardware support flags for an audio endpoint device.
    /// </summary>
    /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370835(v=vs.85).aspx"/>.</remarks>
    public enum EndpointHardwareSupportFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The audio endpoint device supports a hardware volume control.
        /// </summary>
        Volume = 0x1,

        /// <summary>
        /// The audio endpoint device supports a hardware mute control.
        /// </summary>
        Mute = 0x2,

        /// <summary>
        /// The audio endpoint device supports a hardware peak meter.
        /// </summary>
        Meter = 0x4
    }
}