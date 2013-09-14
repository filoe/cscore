using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    public class WaveFormatMarshaler : ICustomMarshaler
    {
        private static WaveFormatMarshaler instance = null;

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return (instance != null) ? instance : new WaveFormatMarshaler();
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            Marshal.FreeHGlobal(pNativeData);
        }

        public int GetNativeDataSize()
        {
            throw new NotImplementedException();
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            return WaveFormatToPointer((WaveFormat)ManagedObj);
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return PointerToWaveFormat(pNativeData);
        }

        public static IntPtr WaveFormatToPointer(WaveFormat format)
        {
            IntPtr formatPointer = Marshal.AllocHGlobal(Marshal.SizeOf(format));
            Marshal.StructureToPtr(format, formatPointer, false);
            return formatPointer;
        }

        public static WaveFormat PointerToWaveFormat(IntPtr pointer)
        {
            WaveFormat waveFormat = (WaveFormat)Marshal.PtrToStructure(pointer, typeof(WaveFormat));
            waveFormat.ExtraSize = 0;
            return waveFormat;
        }
    }
}