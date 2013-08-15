using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct DSBufferDescription
    {
        public int dwSize;

        [MarshalAs(UnmanagedType.U4)]
        public DSBufferCapsFlags dwFlags;

        public uint dwBufferBytes;
        public int dwReserved;
        public IntPtr lpwfxFormat;
        public Guid guid3DAlgorithm;
    }
}