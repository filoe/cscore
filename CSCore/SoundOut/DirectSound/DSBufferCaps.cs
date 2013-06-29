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

    [Flags]
    public enum DSBufferFlags
    {
        Control3D = 0x10,
        ControlEffects = 0x200,
        ControlFrequency = 0x20,
        ControlPan = 0x40,
        ControlPositionNotify = 0x100,
        ControlVolume = 0x80,
        Defer = 0x40000,
        GetCurrentPosition2 = 0x10000,
        GlobalFocus = 0x8000,
        Hardware = 4,
        Mute3DAtMaxDistance = 0x20000,
        None = 0,
        PrimaryBuffer = 1,
        Software = 8,
        Static = 2,
        StickyFocus = 0x4000,
        Trueplayposition = 0x80000
    }
}
