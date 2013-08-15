using CSCore.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    [Guid("657804FA-D6AD-4496-8A60-352752AF4F89")]
    public class AudioEndpointVolumeCallback : IAudioEndpointVolumeCallback
    {
        public event EventHandler<AudioEndpointVolumeCallbackEventArgs> NotifyRecived;

        int IAudioEndpointVolumeCallback.OnNotify(IntPtr notifyData)
        {
            if (notifyData == IntPtr.Zero)
                return (int)HResult.E_INVALIDARG;

            var data = (AudioVolumeNotificationData)Marshal.PtrToStructure(notifyData, typeof(AudioVolumeNotificationData));
            if (NotifyRecived != null)
                NotifyRecived(this, new AudioEndpointVolumeCallbackEventArgs(data, notifyData));

            return (int)HResult.S_OK;
        }
    }
}