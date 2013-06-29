using System.Runtime.InteropServices;

namespace CSCore.Utils
{
    [StructLayout(LayoutKind.Explicit)]
    public struct HightLowConverter<T> where T : struct
    {
        public HightLowConverter(T value)
        {
            Low = 0;
            High = 0;
            Value = value;
        }

        [FieldOffset(0)]
        public T Value;

        [FieldOffset(0)]
        public ushort Low;

        [FieldOffset(2)]
        public ushort High;
    }
}
