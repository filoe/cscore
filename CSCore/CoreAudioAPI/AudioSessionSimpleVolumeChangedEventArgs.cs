using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioSessionSimpleVolumeChanged
    /// </summary>
    public class AudioSessionSimpleVolumeChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// The new volume level for the audio session. This parameter is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume (no attenuation).
        /// </summary>
        public float NewVolume { get; private set; }

        /// <summary>
        /// The new muting state. If true, muting is enabled. If false, muting is disabled.
        /// </summary>
        public bool IsMuted { get; private set; }

        public AudioSessionSimpleVolumeChangedEventArgs(float newVolume, bool isMuted, Guid eventContext)
            : base(eventContext)
        {
            if (newVolume < 0 || newVolume > 1)
                throw new ArgumentOutOfRangeException("newVolume");
            NewVolume = newVolume;
            IsMuted = isMuted;
        }
    }
}