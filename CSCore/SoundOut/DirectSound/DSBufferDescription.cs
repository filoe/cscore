using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
// ReSharper disable once InconsistentNaming
    public struct DSBufferDescription
    {
        public int dwSize;

        [MarshalAs(UnmanagedType.U4)]
        public DSBufferCapsFlags dwFlags;

        public int dwBufferBytes;
        public int dwReserved;
        public IntPtr lpwfxFormat;
        public Guid guid3DAlgorithm;
    }
}