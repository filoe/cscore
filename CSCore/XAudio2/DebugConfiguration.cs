using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Contains the new global debug configuration for XAudio2. Used with the <see cref="XAudio2.SetDebugConfiguration" />
    ///     function.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DebugConfiguration
    {
        /// <summary>
        ///     Bitmask of enabled debug message types. For a list of possible values take look at:
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2_debug_configuration(v=vs.85).aspx.
        /// </summary>
        public LogMask TraceMask;

        /// <summary>
        ///     Message types that will cause an immediate break. For a list of possible values take look at:
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2_debug_configuration(v=vs.85).aspx.
        /// </summary>
        public LogMask BreakMask;

        /// <summary>
        ///     Indicates whether to log the thread ID with each message.
        /// </summary>
        public NativeBool LogThreadId;

        /// <summary>
        ///     Indicates whether to log source files and line numbers.
        /// </summary>
        public NativeBool LogFileline;

        /// <summary>
        ///     Indicates whether to log function names.
        /// </summary>
        public NativeBool LogFunctionName;

        /// <summary>
        ///     Indicates whether to log message timestamps.
        /// </summary>
        public NativeBool LogTiming;
    }
}