using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
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

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String atom, String title);
    }
}