using System;
using System.Runtime.InteropServices;

namespace CSCore.SoundOut.DirectSound
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    public class DSInterop
    {
        public static readonly Guid AllObjects = new Guid("aa114de5-c262-4169-a1c8-23d698cc73b5");

        [DllImport("dsound.dll", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern DSResult DirectSoundCreate(ref Guid GUID, [Out] out IntPtr directSound, IntPtr pUnkOuter);

        public static DSResult DirectSoundCreate(DirectSoundDevice device, out IntPtr directSound)
        {
            Guid guid = device.Guid;
            return DirectSoundCreate(ref guid, out directSound, IntPtr.Zero);
        }

        [DllImport("dsound.dll", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern DSResult DirectSoundCreate8(ref Guid GUID, [Out] out IntPtr directSound, IntPtr pUnkOuter);

        public static DSResult DirectSoundCreate8(DirectSoundDevice device, out IntPtr directSound)
        {
            Guid guid = device.Guid;
            return DirectSoundCreate8(ref guid, out directSound, IntPtr.Zero);
        }

        [DllImport("dsound.dll", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "DirectSoundEnumerateA")]
        public static extern DSResult DirectSoundEnumerate(DSEnumCallback lpDSEnumCallback, IntPtr lpContext);

        public class DirectSoundUtils
        {
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(String atom, String title);

            public static IntPtr GetConsoleHandle()
            {
                return FindWindow(null, Console.Title);
            }
        }
    }
}
