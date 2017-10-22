using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    ///     Provides data for the <see cref="AudioEndpointVolumeCallback.NotifyRecived" /> event.
    /// </summary>
    public class AudioEndpointVolumeCallbackEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AudioEndpointVolumeCallbackEventArgs" /> class.
        /// </summary>
        /// <param name="data">The data which describes a change in the volume level or muting state of an audio endpoint device.</param>
        /// <param name="nativePtr">The native pointer to the <paramref name="data" />.</param>
        public AudioEndpointVolumeCallbackEventArgs(AudioVolumeNotificationData data, IntPtr nativePtr)
        {
            EventContext = data.EventContext;
            IsMuted = data.Muted;
            MasterVolume = data.MasterVolume;
            Channels = data.Channels;
            ChannelVolumes = data.GetAllChannelVolumes();
        }

        /// <summary>
        ///     Gets the event context value.
        /// </summary>
        /// <value>
        ///     The event context value.
        /// </value>
        /// <remarks>
        ///     Context value for the <see cref="IAudioEndpointVolumeCallback.OnNotify" /> method. This member is the value of the
        ///     event-context GUID that was provided as an input parameter to the <see cref="AudioEndpointVolume" /> method call
        ///     that changed the endpoint volume level or muting state. For more information, see
        ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd370799(v=vs.85).aspx" />.
        /// </remarks>
        public Guid EventContext { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the audio stream is currently muted.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the audio stream is currently muted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMuted { get; private set; }

        /// <summary>
        ///     Gets the current master volume level of the audio stream. The volume level is
        ///     normalized to the range from 0.0 to 1.0, where 0.0 is the minimum volume level and 1.0
        ///     is the maximum level. Within this range, the relationship of the normalized volume level
        ///     to the attenuation of signal amplitude is described by a nonlinear, audio-tapered curve.
        /// </summary>
        public float MasterVolume { get; private set; }

        /// <summary>
        ///     Gets the number of channels.
        /// </summary>
        /// <value>
        ///     The number of channels.
        /// </value>
        public int Channels { get; private set; }

        /// <summary>
        ///     Gets the volume level for each channel is normalized to the range from 0.0 to 1.0, where 0.0
        ///     is the minimum volume level and 1.0 is the maximum level. Within this range, the
        ///     relationship of the normalized volume level to the attenuation of signal amplitude is
        ///     described by a nonlinear, audio-tapered curve.
        /// </summary>
        public float[] ChannelVolumes { get; private set; }
    }
}