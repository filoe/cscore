using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    public class AudioEndpointVolumeChannel
    {
        protected AudioEndpointVolume AudioEndpointVolume { get; set; }
        private readonly int _channelIndex;

        public int ChannelIndex
        {
            get { return _channelIndex; }
        }

        public AudioEndpointVolumeChannel(AudioEndpointVolume audioEndpointVolume, int channelIndex)
        {
            if (audioEndpointVolume == null)
                throw new ArgumentNullException("audioEndpointVolume");
            if (channelIndex < 0)
                throw new ArgumentOutOfRangeException("channelIndex");

            AudioEndpointVolume = audioEndpointVolume;
            _channelIndex = channelIndex;
        }

        public float Volume
        {
            get
            {
                return AudioEndpointVolume.GetChannelVolumeLevel(ChannelIndex);
            }
            set
            {
                AudioEndpointVolume.SetChannelVolumeLevel(ChannelIndex, value, Guid.Empty);
            }
        }

        public float VolumeScalar
        {
            get
            {
                return AudioEndpointVolume.GetChannelVolumeLevelScalar(ChannelIndex);
            }
            set
            {
                AudioEndpointVolume.SetChannelVolumeLevelScalar(ChannelIndex, value, Guid.Empty);
            }
        }
    }
}