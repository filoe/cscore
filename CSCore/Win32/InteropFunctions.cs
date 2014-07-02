using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Win32
{
    internal class InteropFunctions
    {
        [DllImport("ole32.dll", ExactSpelling = true)]
        internal static extern HResult CoCreateInstance([MarshalAs(UnmanagedType.LPStruct)] [In] Guid rclsid, IntPtr pUnkOuter, CLSCTX dwClsContext, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid, out IntPtr comObject);

        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto)]
        internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, [Out] StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

        
    }
}
