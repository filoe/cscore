using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Win32
{
    internal class NativeMethods
    {
        [DllImport("ole32.dll", ExactSpelling = true)]
        internal static extern HResult CoCreateInstance([MarshalAs(UnmanagedType.LPStruct)] [In] Guid rclsid, IntPtr pUnkOuter, CLSCTX dwClsContext, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid, out IntPtr comObject);

        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto)]
        internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, [Out] StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal extern static IntPtr LoadLibrary(string librayName);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
