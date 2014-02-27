using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    //http://msdn.microsoft.com/en-us/library/windows/desktop/aa380072(v=vs.85).aspx
    [StructLayout(LayoutKind.Explicit)]
    public struct PropertyVariant : IDisposable
    {
        //todo: create interop class
        [DllImport("ole32.dll")]
        private static extern int PropVariantClear(ref PropertyVariant propertyVariant);

        public static PropertyVariant CreateLong(long value)
        {
            return new PropertyVariant() { HValue = value, DataType = VarEnum.VT_I8 };
        }

        [FieldOffset(0)]
        public short Vartype; //vt

        [FieldOffset(2)]
        public short wReserved1;

        [FieldOffset(4)]
        public short wReserved2;

        [FieldOffset(6)]
        public short wReserved3;

        [FieldOffset(8)]
        public sbyte CValue;

        [FieldOffset(8)]
        public byte BValue;

        [FieldOffset(8)]
        public short IValue;

        [FieldOffset(8)]
        public ushort UIValue;

        [FieldOffset(8)]
        public int LValue;

        [FieldOffset(8)]
        public uint ULValue;

        [FieldOffset(8)]
        public int IntValue;

        [FieldOffset(8)]
        public uint UIntValue;

        [FieldOffset(8)]
        public long HValue;

        [FieldOffset(8)]
        public ulong UHValue;

        [FieldOffset(8)]
        public float FloatValue;

        [FieldOffset(8)]
        public double DoubleValue;

        [FieldOffset(8)]
        public bool BoolValue;

        [FieldOffset(8)]
        public int Scode;

        [FieldOffset(8)]
        public DateTime Date;

        [FieldOffset(8)]
        public System.Runtime.InteropServices.ComTypes.FILETIME FileTime;

        [FieldOffset(8)]
        public Blob BlobValue;

        [FieldOffset(8)]
        public IntPtr PointerValue;

        public VarEnum DataType
        {
            get { return (VarEnum)Vartype; }
            set { Vartype = (short)value; }
        }

        /// <summary>
        /// Warning: May return null if DataType is not supported.
        /// </summary>
        public object GetValue()
        {
            switch (DataType)
            {
                case VarEnum.VT_I1:
                    return BValue;

                case VarEnum.VT_I2:
                    return IValue;

                case VarEnum.VT_I4:
                    return LValue;

                case VarEnum.VT_I8:
                    return HValue;

                case VarEnum.VT_INT:
                    return IValue;

                case VarEnum.VT_UI4:
                    return ULValue;

                case VarEnum.VT_UI8:
                    return UHValue;

                case VarEnum.VT_LPWSTR:
                    return Marshal.PtrToStringUni(PointerValue);

                case VarEnum.VT_BLOB:
                case VarEnum.VT_VECTOR | VarEnum.VT_UI1:
                    return BlobValue.GetData();

                case VarEnum.VT_CLSID:
                    return (Guid)Marshal.PtrToStructure(PointerValue, typeof(Guid));

                case VarEnum.VT_BOOL:
                    return BoolValue;

                default:
                    return null;
                //throw new NotImplementedException("Datatype not supported: " + DataType.ToString());
            }
        }

        public void Dispose()
        {
            Marshal.ThrowExceptionForHR(PropVariantClear(ref this));
        }

        public override string ToString()
        {
            var value = GetValue();
            return value == null ? "null" : value.ToString();
        }
    }
}