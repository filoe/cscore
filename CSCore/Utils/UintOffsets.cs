using System;
using System.Runtime.InteropServices;

namespace CSCore.Utils
{
    [StructLayout(LayoutKind.Explicit)]
    public struct HightLowConverterInt32//<T> where T : struct
    {
        public HightLowConverterInt32(Int32 value)
        {
            Low = 0;
            High = 0;
            Value = value;
        }

        [FieldOffset(0)]
        public Int32 Value;

        [FieldOffset(0)]
        public ushort Low;

        [FieldOffset(2)]
        public ushort High;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct HightLowConverterUInt32//<T> where T : struct
    {
        public HightLowConverterUInt32(UInt32 value)
        {
            Low = 0;
            High = 0;
            Value = value;
        }

        [FieldOffset(0)]
        public UInt32 Value;

        [FieldOffset(0)]
        public ushort Low;

        [FieldOffset(2)]
        public ushort High;
    }
}