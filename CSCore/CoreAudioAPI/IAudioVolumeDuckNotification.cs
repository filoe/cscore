using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.CoreAudioAPI
{
    [ComImport]
    [Guid("C3B284D4-6D39-4359-B3CF-B56DDB3BB39C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IAudioVolumeDuckNotification
    {
        int OnVolumeDuckNotification([In, MarshalAs(UnmanagedType.LPWStr)] string sessionID, int countCommunicationSessions);
        int OnVolumeUnduckNotification([In, MarshalAs(UnmanagedType.LPWStr)] string sessionID, int countCommunicationSessions);
    }
}
