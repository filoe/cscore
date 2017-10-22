using System;
using System.Runtime.InteropServices;

namespace CSCore.DirectSound
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal class NativeMethods
    {
        [DllImport("dsound.dll", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern DSResult DirectSoundCreate(ref Guid GUID, [Out] out IntPtr directSound, IntPtr pUnkOuter);

        [DllImport("dsound.dll", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern DSResult DirectSoundCreate8(ref Guid GUID, [Out] out IntPtr directSound, IntPtr pUnkOuter);

        [DllImport("dsound.dll", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "DirectSoundEnumerateA")]
        public static extern DSResult DirectSoundEnumerate(DSEnumCallback lpDSEnumCallback, IntPtr lpContext);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ThrowOnUnmappableChar = true, BestFitMapping = false)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] String atom, [MarshalAs(UnmanagedType.LPTStr)] String title);
    }
}