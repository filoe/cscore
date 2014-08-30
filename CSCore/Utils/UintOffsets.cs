using System;
using System.Runtime.InteropServices;

namespace CSCore.Utils
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct HightLowConverterInt32
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
    internal struct HightLowConverterUInt32
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