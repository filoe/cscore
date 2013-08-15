using System;

namespace CSCore.CoreAudioAPI
{
    //http://msdn.microsoft.com/en-us/library/windows/desktop/dd371400(v=vs.85).aspx
    //mmdeviceapi.h line 128
    [Flags]
    public enum DeviceState
    {
        Active = 0x00000001,
        Disabled = 0x00000002,
        NotPresent = 0x00000004,
        UnPlugged = 0x00000008,
        All = 0x0000000F
    }
}