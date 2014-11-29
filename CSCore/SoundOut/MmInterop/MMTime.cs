using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.MMInterop
{
    /// <summary>
    /// http: //www.pinvoke.net/default.aspx/Structures/MmTime.html
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct MMTime
    {
        [FieldOffset(0)]
        public MMTimeType wType;

        [FieldOffset(4)]
        public Int32 ms;

        [FieldOffset(4)]
        public Int32 sample;

        [FieldOffset(4)]
        public Int32 cb;

        [FieldOffset(4)]
        public Int32 ticks;

        [FieldOffset(4)]
        public Byte smpteHour;

        [FieldOffset(5)]
        public Byte smpteMin;

        [FieldOffset(6)]
        public Byte smpteSec;

        [FieldOffset(7)]
        public Byte smpteFrame;

        [FieldOffset(8)]
        public Byte smpteFps;

        [FieldOffset(9)]
        public Byte smpteDummy;

        [FieldOffset(10)]
        public Byte smptePad0;

        [FieldOffset(11)]
        public Byte smptePad1;

        [FieldOffset(4)]
        public Int32 midiSongPtrPos;
    }
}