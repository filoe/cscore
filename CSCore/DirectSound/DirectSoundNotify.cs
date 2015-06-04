using System;
using System.Runtime.InteropServices;
using System.Security;
using CSCore.Win32;

namespace CSCore.DirectSound
{
    /// <summary>
    /// Sets up notification events for a playback or capture buffer.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    [Guid("B0210783-89CD-11D0-AF08-00A0C925CD16")]
    public class DirectSoundNotify : ComObject
    {
        /// <summary>
        /// Returns a new instance of the <see cref="DirectSoundNotify"/> class for the specified <paramref name="directSoundBuffer"/>.
        /// </summary>
        /// <param name="directSoundBuffer">The <see cref="DirectSoundBuffer"/> to create a <see cref="DirectSoundNotify"/> instance for.</param>
        /// <returns>A new instance of the <see cref="DirectSoundNotify"/> class for the specified <paramref name="directSoundBuffer"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="directSoundBuffer"/> is null.</exception>
        public static DirectSoundNotify FromBuffer(DirectSoundBuffer directSoundBuffer)
        {
            if (directSoundBuffer == null) 
                throw new ArgumentNullException("directSoundBuffer");
            return directSoundBuffer.QueryInterface<DirectSoundNotify>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSoundNotify"/> class based on the native pointer.
        /// </summary>
        /// <param name="ptr">The native pointer of the COM object.</param>
        public DirectSoundNotify(IntPtr ptr)
            : base(ptr)
        {
        }

        /// <summary>
        /// Sets the notification positions. During capture or playback, whenever the read or play cursor reaches one of the specified offsets, the associated event is signaled. 
        /// </summary>
        /// <param name="notifies">An array of <see cref="DSBPositionNotify"/> structures.</param>
        public void SetNotificationPositions(DSBPositionNotify[] notifies)
        {
            DirectSoundException.Try(SetNotificationPositionsNative(notifies), "IDirectSoundNotify", "SetNotificationPositions");
        }

        /// <summary>
        /// Sets the notification positions. During capture or playback, whenever the read or play cursor reaches one of the specified offsets, the associated event is signaled. 
        /// </summary>
        /// <param name="notifies">An array of <see cref="DSBPositionNotify"/> structures.</param>
        /// <returns>DSResult</returns>
        public unsafe DSResult SetNotificationPositionsNative(DSBPositionNotify[] notifies)
        {
            fixed (void* pnotifies = notifies)
            {
                return InteropCalls.CalliMethodPtr(UnsafeBasePtr, notifies.Length, pnotifies, ((void**)(*(void**)UnsafeBasePtr))[3]);
            }
        }
    }
}