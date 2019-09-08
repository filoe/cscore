using System;
using System.Collections.Generic;
using System.Text;

namespace CSCore
{
    public interface IChannelMapperFactory
    {
        IWaveSource MapChannels(IWaveSource input, DSP.ChannelMatrix channelMatrix);
        IWaveSource MapChannels(IWaveSource input, int targetNumberOfChannels);
        ISampleSource MapChannels(ISampleSource input, DSP.ChannelMatrix channelMatrix);
        ISampleSource MapChannels(ISampleSource input, int targetNumberOfChannels);
    }
}
