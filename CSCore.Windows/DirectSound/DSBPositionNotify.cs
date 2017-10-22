using System;
using System.Runtime.InteropServices;

namespace CSCore.DirectSound
{
    /*
    typedef struct _DSBPOSITIONNOTIFY
    {
        DWORD           dwOffset;
        HANDLE          hEventNotify;
    } DSBPOSITIONNOTIFY, *LPDSBPOSITIONNOTIFY;
     */

    //http://msdn.microsoft.com/en-us/library/ms897759.aspx
    /// <summary>
    /// The <see cref="DSBPositionNotify"/> structure describes a notification position. It is used by <see cref="DirectSoundNotify.SetNotificationPositions"/>. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DSBPositionNotify
    {
        /// <summary>
        /// Zero offset.
        /// </summary>
        public const int OffsetZero = 0x0;
        /// <summary>
        /// Causes the event to be signaled when playback or capture stops, either because the end of the buffer has been reached (and playback or capture is not looping) or because the application called the <see cref="DirectSoundBuffer.Stop"/> or IDirectSoundCaptureBuffer8::Stop method.
        /// </summary>
        public const int OffsetStop = unchecked((int)0xFFFFFFFF);

        /// <summary>
        /// Offset from the beginning of the buffer where the notify event is to be triggered, or <see cref="OffsetStop"/>.
        /// </summary>
        public int Offset;
        /// <summary>
        /// Handle to the event to be signaled when the offset has been reached.
        /// </summary>
        public IntPtr EventNotifyHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DSBPositionNotify"/> struct.
        /// </summary>
        /// <param name="offset">The offset from the beginning of the buffer where the notify event is to be triggered.</param>
        /// <param name="eventNotifyHandle">Handle to the event to be signaled when the offset has been reached</param>
        public DSBPositionNotify(int offset, IntPtr eventNotifyHandle)
        {
            Offset = offset;
            EventNotifyHandle = eventNotifyHandle;
        }
    }
}