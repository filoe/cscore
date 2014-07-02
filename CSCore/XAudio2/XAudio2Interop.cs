using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.XAudio2
{
    [SuppressUnmanagedCodeSecurity]
    internal class XAudio2Interop
    {
        private const string XAudioDll = "xaudio2_8.dll";

        [DllImport(XAudioDll, CallingConvention = CallingConvention.StdCall)]
        internal static extern int XAudio2Create(IntPtr ptr, int flags, XAudio2Processor flags0);
    }
}