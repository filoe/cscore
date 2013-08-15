using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    [Guid("045FA593-8799-42b8-BC8D-8968C6453507")]
    public class MFMediaBuffer : ComObject
    {
        private const string c = "IMFMediaBuffer";

        public int CurrentLength
        {
            get { return GetCurrentLength(); }
            set { SetCurrentLength(value); }
        }

        public int MaxLength
        {
            get { return GetMaxLength(); }
        }

        public MFMediaBuffer(IntPtr ptr)
            : base(ptr)
        {
        }

        public unsafe int LockNative(out IntPtr buffer, out int maxLength, out int currentLength)
        {
            fixed (void* pbuffer = &buffer, pmaxlength = &maxLength, pcurrentlength = &currentLength)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, pbuffer, pmaxlength, pcurrentlength, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        public IntPtr Lock(out int maxLength, out int currentLength)
        {
            IntPtr p = IntPtr.Zero;
            MediaFoundationException.Try(LockNative(out p, out maxLength, out currentLength), c, "Lock");
            return p;
        }

        public unsafe int UnlockNative()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[4]);
        }

        public void Unlock()
        {
            MediaFoundationException.Try(UnlockNative(), c, "Unlock");
        }

        public unsafe int GetCurrentLengthNative(out int currentLength)
        {
            fixed (void* ptr = &currentLength)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, ptr, ((void**)(*(void**)_basePtr))[5]);
            }
        }

        public int GetCurrentLength()
        {
            int res;
            MediaFoundationException.Try(GetCurrentLengthNative(out res), c, "GetCurrentLength");
            return res;
        }

        public unsafe int SetCurrentLengthNative(int currentLength)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, currentLength, ((void**)(*(void**)_basePtr))[6]);
        }

        public void SetCurrentLength(int currentLength)
        {
            MediaFoundationException.Try(SetCurrentLengthNative(currentLength), c, "SetCurrentLength");
        }

        public unsafe int GetMaxLengthNative(out int maxlength)
        {
            fixed (void* ptr = &maxlength)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, ptr, ((void**)(*(void**)_basePtr))[7]);
            }
        }

        public unsafe int GetMaxLength()
        {
            int maxlength;
            MediaFoundationException.Try(GetMaxLengthNative(out maxlength), c, "GetMaxLength");
            return maxlength;
        }
    }
}