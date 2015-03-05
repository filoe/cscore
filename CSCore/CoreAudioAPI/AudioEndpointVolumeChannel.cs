using System;

namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// Represents a single audio endpoint volume channel.
    /// </summary>
    public class AudioEndpointVolumeChannel
    {
        /// <summary>
        /// Gets the parent <see cref="CoreAudioAPI.AudioEndpointVolume"/> instance.
        /// </summary>
        /// <value>
        /// The parent <see cref="CoreAudioAPI.AudioEndpointVolume"/> instance.
        /// </value>
        public AudioEndpointVolume AudioEndpointVolume { get; private set; }

        private readonly int _channelIndex;

        /// <summary>
        /// Gets the index of the audio endpoint channel.
        /// </summary>
        /// <value>
        /// The index of the audio endpoint channel.
        /// </value>
        public int ChannelIndex
        {
            get { return _channelIndex; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioEndpointVolumeChannel"/> class.
        /// </summary>
        /// <param name="audioEndpointVolume">The underlying <see cref="CoreAudioAPI.AudioEndpointVolume"/> which provides access to the audio endpoint volume.</param>
        /// <param name="channelIndex">The zero-based index of the channel.</param>
        public AudioEndpointVolumeChannel(AudioEndpointVolume audioEndpointVolume, int channelIndex)
        {
            if (audioEndpointVolume == null)
                throw new ArgumentNullException("audioEndpointVolume");
            if (channelIndex < 0)
                throw new ArgumentOutOfRangeException("channelIndex");

            AudioEndpointVolume = audioEndpointVolume;
            _channelIndex = channelIndex;
        }

        /// <summary>
        /// Gets or sets the volume in decibel.
        /// </summary>
        /// <value>
        /// The volume in decibel.
        /// </value>
        public float Volume
        {
            get { return AudioEndpointVolume.GetChannelVolumeLevel(ChannelIndex); }
            set { AudioEndpointVolume.SetChannelVolumeLevel(ChannelIndex, value, Guid.Empty); }
        }

        /// <summary>
        /// Gets or sets the volume as a normalized value in the range from 0.0 to 1.0.
        /// </summary>
        /// <value>
        /// The volume as a normalized value in the range from 0.0 to 1.0.
        /// </value>
        public float VolumeScalar
        {
            get { return AudioEndpointVolume.GetChannelVolumeLevelScalar(ChannelIndex); }
            set { AudioEndpointVolume.SetChannelVolumeLevelScalar(ChannelIndex, value, Guid.Empty); }
        }
    }
}