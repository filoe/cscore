using System;

namespace CSCore.DirectSound
{
    internal delegate bool DSEnumCallback(IntPtr lpGuid, IntPtr lpcstrDescription, IntPtr lpstrModule, IntPtr lpContext);
}