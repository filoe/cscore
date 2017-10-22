using System;

namespace CSCore.DirectSound
{
    internal static class DSUtils
    {
        public static readonly Guid AllObjects = new Guid("aa114de5-c262-4169-a1c8-23d698cc73b5");

        public static DSResult DirectSoundCreate(DirectSoundDevice device, out IntPtr directSound)
        {
            Guid guid = device.Guid;
            return NativeMethods.DirectSoundCreate(ref guid, out directSound, IntPtr.Zero);
        }

        public static DSResult DirectSoundCreate8(DirectSoundDevice device, out IntPtr directSound)
        {
            Guid guid = device.Guid;
            return NativeMethods.DirectSoundCreate8(ref guid, out directSound, IntPtr.Zero);
        }

        public static IntPtr GetConsoleHandle()
        {
            return FindWindow(Console.Title);
        }

        public static IntPtr FindWindow(string atom, string windowTitle)
        {
            return NativeMethods.FindWindow(atom, windowTitle);
        }

        public static IntPtr FindWindow(string windowTitle)
        {
            return NativeMethods.FindWindow(null, windowTitle);
        }

        public static IntPtr GetDesktopWindow()
        {
            return NativeMethods.GetDesktopWindow();
        }
    }
}
