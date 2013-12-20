using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    /*
    typedef struct _DSBPOSITIONNOTIFY
    {
        DWORD           dwOffset;
        HANDLE          hEventNotify;
    } DSBPOSITIONNOTIFY, *LPDSBPOSITIONNOTIFY;
     */

    //http://msdn.microsoft.com/en-us/library/ms897759.aspx
    [StructLayout(LayoutKind.Sequential)]
    public struct DSBPositionNotify
    {
        public const uint OffsetZero = 0x0;
        public const uint OffsetEnd = 0xFFFFFFFF;

        public uint dwOffset;
        public IntPtr hEventNotify;

        public DSBPositionNotify(uint dwOffset, IntPtr hEventNotify)
        {
            this.dwOffset = dwOffset;
            this.hEventNotify = hEventNotify;
        }
    }
}