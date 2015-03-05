using CSCore.Win32;
using System;
using System.Runtime.InteropServices;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides an implementation of the <see cref="IAudioEndpointVolumeCallback"/> interface.
    /// </summary>
    [Guid("657804FA-D6AD-4496-8A60-352752AF4F89")]
    public sealed class AudioEndpointVolumeCallback : IAudioEndpointVolumeCallback
    {
        /// <summary>
        /// Occurs when the volume level or the muting state of the audio endpoint device has changed.
        /// </summary>
        public event EventHandler<AudioEndpointVolumeCallbackEventArgs> NotifyRecived;

        /// <summary>
        /// The <see cref="IAudioEndpointVolumeCallback.OnNotify"/> method notifies the client that the volume level or muting state of the audio endpoint device has changed.
        /// </summary>
        /// <param name="notifyData">Pointer to the volume-notification data.</param>
        /// <returns>HRESULT; If the method succeeds, it returns <see cref="HResult.S_OK"/>. If it fails, it returns an error code.</returns>
        int IAudioEndpointVolumeCallback.OnNotify(IntPtr notifyData)
        {
            if (notifyData == IntPtr.Zero)
                return (int) HResult.E_INVALIDARG;

            var data =
                (AudioVolumeNotificationData) Marshal.PtrToStructure(notifyData, typeof (AudioVolumeNotificationData));
            if (NotifyRecived != null)
                NotifyRecived(this, new AudioEndpointVolumeCallbackEventArgs(data, notifyData));

            return (int) HResult.S_OK;
        }
    }
}