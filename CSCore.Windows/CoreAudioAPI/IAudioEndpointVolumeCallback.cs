using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// The <see cref="IAudioEndpointVolumeCallback"/> interface provides notifications of changes in the volume level and muting state of an audio endpoint device. 
    /// </summary>
    [ComImport]
    [Guid("657804FA-D6AD-4496-8A60-352752AF4F89")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.Security.SuppressUnmanagedCodeSecurity]
    public interface IAudioEndpointVolumeCallback
    {
        /// <summary>
        /// Notifies the client that the volume level or muting state of the audio endpoint device has changed.
        /// </summary>
        /// <param name="notifyData">Pointer to the volume-notification data.</param>
        /// <returns>HRESULT; If the method succeeds, it returns <see cref="HResult.S_OK"/>. If it fails, it returns an error code.</returns>
        [PreserveSig]
        int OnNotify(IntPtr notifyData);
    }
}