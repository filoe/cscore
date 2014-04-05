using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CSCore.DMO
{
    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd390167(v=vs.85).aspx
    //(Mediaobj.h) -> Dmo.h
    [ComImport]
    [Guid("59eff8b9-938c-4a26-82f2-95cb84cdc837")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IMediaBuffer
    {
        [PreserveSig]
        int SetLength(int length);

        [PreserveSig]
        int GetMaxLength(out int length);

        [PreserveSig]
        int GetBufferAndLength(IntPtr ppBuffer, IntPtr validDataByteLength);
    }
}
