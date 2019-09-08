using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.DSP;

namespace CSCore.Windows
{
    internal class WindowsChannelMapperFactory : IChannelMapperFactory
    {
        public IWaveSource MapChannels(IWaveSource input, ChannelMatrix channelMatrix)
        {
            return new DmoChannelResampler(input, channelMatrix);
        }

        public ISampleSource MapChannels(ISampleSource input, ChannelMatrix channelMatrix)
        {
            return new DmoChannelResampler(input.ToWaveSource(), channelMatrix)
                .ToSampleSource();
        }

        public IWaveSource MapChannels(IWaveSource input, int targetNumberOfChannels)
        {
            WaveFormat waveFormat = (WaveFormat)input.WaveFormat.Clone();
            waveFormat.Channels = 2;
            return new DmoResampler(input, waveFormat);
        }

        public ISampleSource MapChannels(ISampleSource input, int targetNumberOfChannels)
        {
            var waveSource = input.ToWaveSource();
            WaveFormat waveFormat = (WaveFormat)waveSource.WaveFormat.Clone();
            waveFormat.Channels = 2;
            return new DmoResampler(waveSource, waveFormat)
                .ToSampleSource();
        }
    }
}
