using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.MMInterop
{
    [StructLayout(LayoutKind.Sequential)]
    public class WaveHeader
    {
        /// <summary>
        /// pointer to locked data buffer (lpData)
        /// </summary>
        public IntPtr dataBuffer;

        /// <summary>
        /// length of data buffer (dwBufferLength)
        /// </summary>
        public int bufferLength;

        /// <summary>
        /// used for input only (dwBytesRecorded)
        /// </summary>
        public int bytesRecorded;

        /// <summary>
        /// for client's use (dwUser)
        /// </summary>
        public IntPtr userData;

        /// <summary>
        /// assorted flags (dwFlags)
        /// </summary>
        public WaveHeaderFlags flags;

        /// <summary>
        /// loop control counter (dwLoops)
        /// </summary>
        public int loops;

        /// <summary>
        /// PWaveHdr, reserved for driver (lpNext)
        /// </summary>
        public IntPtr next;

        /// <summary>
        /// reserved for driver
        /// </summary>
        public IntPtr reserved;
    }

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