using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    public class MFSourceReader : ComObject
    {
        const string c = "IMFSourceReader";

        public MFSourceReader(IntPtr ptr)
            : base(ptr)
        {
        }

        public unsafe int GetStreamSelection(int streamIndex, out NativeBool selectedRef)
        {
            fixed (NativeBool* p = (&selectedRef))
            {
                return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, (IntPtr*)p, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        public NativeBool GetStreamSelection(int streamIndex)
        {
            NativeBool result;
            MediaFoundationException.Try(GetStreamSelection(streamIndex, out result), c, "GetStreamSelection");
            return result;
        }
        
        public unsafe int SetStreamSelectionNative(int streamIndex, NativeBool selected)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, streamIndex, selected, ((void**)(*(void**)_basePtr))[4]);
        }

        public void SetStreamSelection(int streamIndex, NativeBool selected)
        {
            MediaFoundationException.Try(SetStreamSelectionNative(streamIndex, selected), c, "SetStreamSelection");
        }


    }
}
