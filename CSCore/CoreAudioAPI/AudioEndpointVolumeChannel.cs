using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    public class AudioEndpointVolumeChannel
    {
        protected AudioEndpointVolume _audioEndpointVolume;
        private int _channelIndex;

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

            _audioEndpointVolume = audioEndpointVolume;
            _channelIndex = channelIndex;
        }

        public float Volume
        {
            get
            {
                return _audioEndpointVolume.GetChannelVolumeLevel((uint)ChannelIndex);
            }
            set
            {
                _audioEndpointVolume.SetChannelVolumeLevel((uint)ChannelIndex, value, Guid.Empty);
            }
        }

        public float VolumeScalar
        {
            get
            {
                return _audioEndpointVolume.GetChannelVolumeLevelScalar((uint)ChannelIndex);
            }
            set
            {
                _audioEndpointVolume.SetChannelVolumeLevelScalar((uint)ChannelIndex, value, Guid.Empty);
            }
        }
    }
}