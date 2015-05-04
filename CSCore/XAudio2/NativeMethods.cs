using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.XAudio2
{
    internal class NativeMethods
    {
        private const string XAudioDll = "xaudio2_8.dll";

        [SuppressUnmanagedCodeSecurity]
        [DllImport(XAudioDll, CallingConvention = CallingConvention.StdCall)]
        internal static extern int XAudio2Create(IntPtr ptr, int flags, XAudio2Processor flags0);
    }
}