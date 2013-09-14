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

        public unsafe DSResult SetNotificationPositions(IEnumerable<DSBPositionNotify> notifies)
        {
            DSBPositionNotify[] a_notifies = notifies.ToArray();
            fixed (void* pnotifies = a_notifies)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, a_notifies.Length, pnotifies, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        public DSResult SetNotificationPositions(DSBPositionNotify[] notifies)
        {
            return SetNotificationPositions(notifies as IEnumerable<DSBPositionNotify>);
        }

        protected override bool AssertOnNoDispose()
        {
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            //base.Dispose(disposing); todo: 
        }
    }
}