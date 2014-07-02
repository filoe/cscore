using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.MediaFoundation
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("ad4c1b00-4bf7-422f-9175-756693d9130d")]
    public interface IMFByteStream : IUnknown
    {
        [PreserveSig]
        int GetCapabilities(ref MFByteStreamCapsFlags capabilites);

        [PreserveSig]
        int GetLength(ref long length);

        [PreserveSig]
        int SetLength(long length);

        [PreserveSig]
        int GetCurrentPosition(ref long position);

        [PreserveSig]
        int SetCurrentPosition(long position);

        [PreserveSig]
        int IsEndOfStream([MarshalAs(UnmanagedType.Bool)] ref bool endOfStream);

        [PreserveSig]
        int Read(IntPtr buffer, int count, ref int readCount);

        [PreserveSig]
        int BeginRead(IntPtr buffer, int count, IntPtr callback, IntPtr punkState);

        [PreserveSig]
        int EndRead(IntPtr result, ref int readCount);

        [PreserveSig]
        int Write(IntPtr buffer, int count, ref int writtenCount);

        [PreserveSig]
        int BeginWrite(IntPtr buffer, int count, IntPtr callback, IntPtr punkState);

        [PreserveSig]
        int EndWrite(IntPtr result, ref int writtenCount);

        [PreserveSig]
        int Seek(int seekOrigin, long seekOffset, int seekFlags, ref long currentPosition);

        [PreserveSig]
        int Flush();

        [PreserveSig]
        int Close();
    }
}