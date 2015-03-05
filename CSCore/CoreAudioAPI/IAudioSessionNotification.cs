using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="IAudioSessionNotification"/> interface provides notification when an audio session is created.
    /// </summary>
    [ComImport]
    [Guid("641DD20B-4D41-49CC-ABA3-174B9477BB08")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IAudioSessionNotification
    {
        /// <summary>
        /// Notifies the registered processes that the audio session has been created.
        /// </summary>
        /// <param name="newSession">Pointer to the <see cref="AudioSessionControl"/> object of the audio session that was created.</param>
        /// <returns>HRESULT</returns>
        [PreserveSig]
        int OnSessionCreated([In] IntPtr newSession); //newSession is a pointer to an IAudioSessionControl interface
    }
}