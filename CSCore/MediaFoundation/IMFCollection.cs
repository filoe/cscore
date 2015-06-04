using System;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    [ComImport]
    [Guid("5BC8A76B-869A-46a3-9B03-FA218A66AEBE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal interface IMFCollection
    {
        int GetElementCount([Out] out int count);

        int GetElement([In] int elementIndex, [Out] out IntPtr element);

        int AddElement([In] IntPtr element);

        int RemoveElement([In] int elementIndex, [Out] out IntPtr element);

        int InsertElementAt([In] int index, [In] IntPtr element);

        int RemoveAllElements();
    }
}