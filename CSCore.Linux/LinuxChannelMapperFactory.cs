using System;
using System.Collections.Generic;
using System.Text;
using CSCore.DSP;

namespace CSCore.Linux
{
    internal class LinuxChannelMapperFactory : IChannelMapperFactory
    {
        public IWaveSource MapChannels(IWaveSource input, ChannelMatrix channelMatrix) 
            => throw new PlatformNotSupportedException("ChannelMapping is currently not supported on this platform.");

        public IWaveSource MapChannels(IWaveSource input, int targetNumberOfChannels)
            => throw new PlatformNotSupportedException("ChannelMapping is currently not supported on this platform.");

        public ISampleSource MapChannels(ISampleSource input, ChannelMatrix channelMatrix)
            => throw new PlatformNotSupportedException("ChannelMapping is currently not supported on this platform.");

        public ISampleSource MapChannels(ISampleSource input, int targetNumberOfChannels)
            => throw new PlatformNotSupportedException("ChannelMapping is currently not supported on this platform.");
    }
}
