using System;
using System.Collections.Generic;
using System.Text;

namespace CSCore
{
    public interface IResamplerFactory
    {
        IWaveSource CreateResampler(IWaveSource waveSource, int targetSampleRate);
        ISampleSource CreateResampler(ISampleSource sampleSource, int targetSampleRate);
    }
}
