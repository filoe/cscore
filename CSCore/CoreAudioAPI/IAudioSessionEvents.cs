using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    [ComImport]
    [Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IAudioSessionEvents
    {
        int OnDisplayNameChanged([In, MarshalAs(UnmanagedType.LPWStr)] string newDisplayName, [In] ref Guid eventContext);
        int OnIconPathChanged([In, MarshalAs(UnmanagedType.LPWStr)] string newIconPath, [In] ref Guid eventContext);
        int OnSimpleVolumeChanged([In] float newVolume, [In, MarshalAs(UnmanagedType.Bool)] bool newMute, [In] ref Guid eventContext);
        int OnChannelVolumeChanged([In] int channelCount, [In] float[] newChannelVolumeArray, [In] int changedChannel, [In] ref Guid eventContext);
        int OnGroupingParamChanged([In] ref Guid newGroupingParam, [In] ref Guid eventContext);
        int OnStateChanged([In] AudioSessionState newState);
        int OnSessionDisconnected([In] AudioSessionDisconnectReason disconnectReason);
    }
}
