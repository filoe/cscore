using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Provides data for the <see cref="AudioSessionEvents.SimpleVolumeChanged"/> event.
    /// </summary>
    public class AudioSessionSimpleVolumeChangedEventArgs : AudioSessionEventContextEventArgs
    {
        /// <summary>
        /// Gets the new volume level for the audio session. 
        /// </summary>
        /// <remarks>The value is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume (no attenuation).</remarks>
        public float NewVolume { get; private set; }

        /// <summary>
        /// Gets the new muting state.
        /// </summary>
        /// <remarks>If true, muting is enabled. If false, muting is disabled.</remarks>
        public bool IsMuted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSessionSimpleVolumeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newVolume">The new volume level for the audio session. This parameter is a value in the range 0.0 to 1.0, where 0.0 is silence and 1.0 is full volume (no attenuation).</param>
        /// <param name="isMuted">The muting state. If true, muting is enabled. If false, muting is disabled.</param>
        /// <param name="eventContext">The event context value.</param>
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