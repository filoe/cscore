using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     Specifies characteristics that a client can assign to an audio stream during the initialization of the stream.
    /// </summary>
    [Flags]
    public enum AudioClientStreamFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     The audio stream will be a member of a cross-process audio session. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370791(v=vs.85).aspx" />.
        /// </summary>
        StreamFlagsCrossProcess = 0x00010000,

        /// <summary>
        ///     The audio stream will operate in loopback mode. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370791(v=vs.85).aspx" />.
        /// </summary>
        StreamFlagsLoopback = 0x00020000,

        /// <summary>
        ///     Processing of the audio buffer by the client will be event driven. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370791(v=vs.85).aspx" />.
        /// </summary>
        StreamFlagsEventCallback = 0x00040000,

        /// <summary>
        ///     The volume and mute settings for an audio session will not persist across system restarts. For more information,
        ///     see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370791(v=vs.85).aspx" />.
        /// </summary>
        StreamFlagsNoPersist = 0x00080000,

        /// <summary>
        ///     This constant is new in Windows 7. The sample rate of the stream is adjusted to a rate specified by an application.
        ///     For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370791(v=vs.85).aspx" />.
        /// </summary>
        StreamFlagsRateAdjust = 0x00100000,
    }
}