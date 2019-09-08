using CSCore.DSP.Resampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Linux
{
    internal class LinuxResamplerFactory : IResamplerFactory
    {
        public IWaveSource CreateResampler(IWaveSource waveSource, int targetSampleRate)
        {
            return new SimpleResampler(waveSource.ToSampleSource(), targetSampleRate)
                .ToWaveSource(waveSource.WaveFormat.BitsPerSample);
        }

        public ISampleSource CreateResampler(ISampleSource sampleSource, int targetSampleRate)
        {
            return new SimpleResampler(sampleSource, targetSampleRate);
        }
    }
}
