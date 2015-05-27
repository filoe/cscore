using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    /// <summary>
    /// The <see cref="PropertyVariant"/> structure is used to store data.
    /// </summary>
    /// <remarks>For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/aa380072(v=vs.85).aspx"/>.</remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct PropertyVariant : IDisposable
    {
        /// <summary>
        /// Value type tag.
        /// </summary>
        [FieldOffset(0)]
        public short Vartype;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        [FieldOffset(2)]
        public short Reserved1;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        [FieldOffset(4)]
        public short Reserved2;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        [FieldOffset(6)]
        public short Reserved3;

        /// <summary>
        /// VT_I1, Version 1
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(8)]
        public sbyte CValue;

        /// <summary>
        /// VT_UI1
        /// </summary>
        [FieldOffset(8)]
        public byte BValue;

        /// <summary>
        /// VT_I2
        /// </summary>
        [FieldOffset(8)]
        public short IValue;

        /// <summary>
        /// VT_UI2
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(8)]
        public ushort UIValue;

        /// <summary>
        /// VT_I4
        /// </summary>
        [FieldOffset(8)]
        public int LValue;

        /// <summary>
        /// VT_UI4
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(8)]
        public uint ULValue;

        /// <summary>
        /// VT_INT, Version 1
        /// </summary>
        [FieldOffset(8)]
        public int IntValue;

        /// <summary>
        /// VT_UINT, Version 1
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(8)]
        public uint UIntValue;

        /// <summary>
        /// VT_I8
        /// </summary>
        [FieldOffset(8)]
        public long HValue;

        /// <summary>
        /// VT_UI8
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(8)]
        public ulong UHValue;

        /// <summary>
        /// VT_R4
        /// </summary>
        [FieldOffset(8)]
        public float FloatValue;

        /// <summary>
        /// VT_R8
        /// </summary>
        [FieldOffset(8)]
        public double DoubleValue;

        /// <summary>
        /// VT_BOOL
        /// </summary>
        [FieldOffset(8)]
        public bool BoolValue;

        /// <summary>
        /// VT_ERROR
        /// </summary>
        [FieldOffset(8)]
        public int SCode;

        /// <summary>
        /// VT_DATE
        /// </summary>
        [FieldOffset(8)]
        public DateTime Date;

        /// <summary>
        /// VT_FILETIME
        /// </summary>
        [FieldOffset(8)]
        public System.Runtime.InteropServices.ComTypes.FILETIME FileTime;

        /// <summary>
        /// VT_BLOB
        /// </summary>
        [FieldOffset(8)]
        public Blob BlobValue;

        /// <summary>
        /// VT_PTR
        /// </summary>
        [FieldOffset(8)]
        public IntPtr PointerValue;

        /// <summary>
        /// Gets or sets the datatype of the <see cref="PropertyVariant"/>.
        /// </summary>
        public VarEnum DataType
        {
            get { return (VarEnum)Vartype; }
            set { Vartype = (short)value; }
        }

        /// <summary>
        /// Returns the associated value of the <see cref="PropertyVariant"/>. The type of the returned value is defined through the <see cref="DataType"/> property.
        /// </summary>
        /// <returns>The associated value of the <see cref="PropertyVariant"/>. If the datatype is not supported, the <see cref="GetValue"/> method will return null.</returns>
        /// <remarks>Not all datatypes are supported.</remarks>
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

                case VarEnum.VT_R8:
                    return DoubleValue;

                case VarEnum.VT_R4:
                    return FloatValue;

                case VarEnum.VT_FILETIME:
                    return FileTime;

                case VarEnum.VT_LPWSTR:
                    return Marshal.PtrToStringUni(PointerValue);

                case VarEnum.VT_BLOB:
                case VarEnum.VT_VECTOR | VarEnum.VT_UI1:
                    return BlobValue.GetData();

                case VarEnum.VT_CLSID:
                    return (Guid)Marshal.PtrToStructure(PointerValue, typeof(Guid));

                case VarEnum.VT_BOOL:
                    return BoolValue;

                case VarEnum.VT_PTR:
                    return PointerValue;

                case VarEnum.VT_DATE:
                    return Date;

                case VarEnum.VT_ERROR:
                    return SCode;

                default:
                    return null;
                //throw new NotImplementedException("Datatype not supported: " + DataType.ToString());
            }
        }

        /// <summary>
        /// Releases the associated memory by calling the PropVariantClear function.
        /// </summary>
        public void Dispose()
        {
            Marshal.ThrowExceptionForHR(NativeMethods.PropVariantClear(ref this));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the value of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents the value of this instance.
        /// </returns>
        public override string ToString()
        {
            var value = GetValue();
            return value == null ? "null" : value.ToString();
        }
    }
}