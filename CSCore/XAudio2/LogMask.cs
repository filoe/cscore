using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Specifies values for the <see cref="DebugConfiguration.TraceMask" /> and
    ///     <see cref="DebugConfiguration.BreakMask" />.
    /// </summary>
    [Flags]
    public enum LogMask
    {
        /// <summary>
        ///     Log nothing.
        /// </summary>
        LogNothing = 0x0000,

        /// <summary>
        ///     Log error messages.
        /// </summary>
        LogErrors = 0x0001,

        /// <summary>
        ///     Log warning messages. Note: Enabling <see cref="LogWarnings" /> also enables <see cref="LogErrors" />.
        /// </summary>
        LogWarnings = 0x0002,

        /// <summary>
        ///     Log informational messages.
        /// </summary>
        LogInfo = 0x0004,

        /// <summary>
        ///     Log detailed informational messages. Note: Enabling <see cref="LogDetail" /> also enables <see cref="LogInfo" />.
        /// </summary>
        LogDetail = 0x0008,

        /// <summary>
        ///     Log public API function entries and exits.
        /// </summary>
        LogApiCalls = 0x0010,

        /// <summary>
        ///     Log internal function entries and exits. Note: Enabling <see cref="LogFuncCalls" /> also enables
        ///     <see cref="LogApiCalls" />.
        /// </summary>
        LogFuncCalls = 0x0020,

        /// <summary>
        ///     Log delays detected and other timing data.
        /// </summary>
        LogTiming = 0x0040,

        /// <summary>
        ///     Log usage of critical sections and mutexes.
        /// </summary>
        LogLocks = 0x0080,

        /// <summary>
        ///     Log memory heap usage information.
        /// </summary>
        LogMemory = 0x0100,

        /// <summary>
        ///     Log audio streaming information.
        /// </summary>
        LogStreaming = 0x1000
    }
}