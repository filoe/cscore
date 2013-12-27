using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    [ComImport]
    [Guid("641DD20B-4D41-49CC-ABA3-174B9477BB08")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IAudioSessionNotification
    {
        int OnSessionCreated(IntPtr newSession); //newSession is a pointer to an IAudioSessionControl interface
    }
}
