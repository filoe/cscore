using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("B0210783-89CD-11D0-AF08-00A0C925CD16")]
    public class DirectSoundNotify : ComObject
    {
        public static DirectSoundNotify FromBuffer(DirectSoundBufferBase buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return buffer.QueryInterface<DirectSoundNotify>();
        }

        public DirectSoundNotify(IntPtr ptr)
            : base(ptr)
        {
        }

        public void SetNotificationPositions(DSBPositionNotify[] notifies)
        {
            DirectSoundException.Try(SetNotificationPositionsNative(notifies), "IDirectSoundNotify", "SetNotificationPositions");
        }

        public unsafe DSResult SetNotificationPositionsNative(DSBPositionNotify[] notifies)
        {
            fixed (void* pnotifies = notifies)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, notifies.Length, pnotifies, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        protected override bool AssertOnNoDispose()
        {
            return false;
        }
    }
}