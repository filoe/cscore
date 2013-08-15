using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DSEffectDesc
    {
        public int Size;
        public DSEffectFlags Flags;
        public Guid Guid;
        public IntPtr Reserved1;
        public IntPtr Reserved2;

        public DSEffectDesc(Guid guid, DSEffectFlags flags)
        {
            Size = Marshal.SizeOf(typeof(DSEffectDesc));
            Flags = flags;
            Guid = guid;
            Reserved1 = IntPtr.Zero;
            Reserved2 = IntPtr.Zero;
        }
    }
}