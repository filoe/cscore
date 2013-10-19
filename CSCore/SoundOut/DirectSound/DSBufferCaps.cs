using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    //http://msdn.microsoft.com/en-us/library/ms897756.aspx
    [StructLayout(LayoutKind.Sequential)]
    public struct DSBufferCaps
    {
        public int dwSize;
        public DSBufferFlags dwFlags;
        public int dwBufferBytes;
        public int dwUnlockTransferRate;
        public int dwPlayCpuOverhead;
    }
}