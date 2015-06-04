using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.Win32
{
    internal class NativeMethods
    {
        [DllImport("ole32.dll", ExactSpelling = true)]
        internal static extern HResult CoCreateInstance([MarshalAs(UnmanagedType.LPStruct)] [In] Guid rclsid, IntPtr pUnkOuter, CLSCTX dwClsContext, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid, out IntPtr comObject);

        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
        internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal extern static IntPtr LoadLibrary(string librayName);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);

        [DllImport("Avrt.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr AvSetMmThreadCharacteristics([MarshalAs(UnmanagedType.LPWStr)] string proAudio, out int taskIndex);

        [DllImport("Avrt.dll")]
        internal static extern bool AvRevertMmThreadCharacteristics(IntPtr avrtHandle);

        [DllImport("ole32.dll")]
        public static extern int PropVariantClear(ref PropertyVariant propertyVariant);
    }
}
