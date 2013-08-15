using System;

namespace CSCore.SoundOut.DirectSound
{
    public delegate bool DSEnumCallback(IntPtr lpGuid, IntPtr lpcstrDescription, IntPtr lpstrModule, IntPtr lpContext);
}