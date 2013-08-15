using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    [Guid("00000000-0000-0000-C000-000000000046")]
    public interface IUnknown
    {
        int QueryInterface(ref Guid giid, out IntPtr ppvObject);

        int AddRef();

        int Release();
    }
}