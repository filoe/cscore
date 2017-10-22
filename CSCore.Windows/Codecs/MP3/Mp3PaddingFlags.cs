using System;

namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// Indicates whether padding is used to adjust the average bitrate to the sampling rate. Use one of the following values:
    /// </summary>
    [Flags]
    public enum Mp3PaddingFlags
    {
        /// <summary>
        /// Insert padding as needed to achieve the stated average bitrate.
        /// </summary>
        PaddingIso = 0,
        /// <summary>
        /// Always insert padding. The average bit rate may be higher than stated.
        /// </summary>
        PaddingOn = 1,
        /// <summary>
        /// Never insert padding. The average bit rate may be lower than stated.
        /// </summary>
        PaddingOff = 2,
    }
}