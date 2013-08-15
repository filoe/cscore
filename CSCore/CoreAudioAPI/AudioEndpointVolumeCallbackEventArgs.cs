using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    public class AudioEndpointVolumeCallbackEventArgs : EventArgs
    {
        public Guid EventContext { get; private set; }

        public bool Muted { get; private set; }

        /// <summary>
        /// Specifies the current master volume level of the audio stream. The volume level is
        /// normalized to the range from 0.0 to 1.0, where 0.0 is the minimum volume level and 1.0
        /// is the maximum level. Within this range, the relationship of the normalized volume level
        /// to the attenuation of signal amplitude is described by a nonlinear, audio-tapered curve.
        /// </summary>
        public float MasterVolume { get; private set; }

        public uint Channels { get; private set; }

        /// <summary>
        /// The volume level for each channel is normalized to the range from 0.0 to 1.0, where 0.0
        /// is the minimum volume level and 1.0 is the maximum level. Within this range, the
        /// relationship of the normalized volume level to the attenuation of signal amplitude is
        /// described by a nonlinear, audio-tapered curve.
        /// </summary>
        public float[] ChannelVolumes { get; private set; }

        public AudioEndpointVolumeCallbackEventArgs(AudioVolumeNotificationData data, IntPtr nativePtr)
        {
            EventContext = data.EventContext;
            Muted = data.Muted;
            MasterVolume = data.MasterVolume;
            Channels = data.Channels;
            ChannelVolumes = data.GetAllChannelVolumes(nativePtr);
        }
    }
}