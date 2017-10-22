using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    internal class WaveFormatMarshaler : ICustomMarshaler
    {
        private static readonly WaveFormatMarshaler Instance = null;

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return Instance ?? new WaveFormatMarshaler();
        }

        public void CleanUpManagedData(object managedObj)
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

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            return WaveFormatToPointer((WaveFormat)managedObj);
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
            if (waveFormat.WaveFormatTag == AudioEncoding.Extensible)
                waveFormat = (WaveFormatExtensible) Marshal.PtrToStructure(pointer, typeof (WaveFormatExtensible));
            return waveFormat;
        }
    }
}