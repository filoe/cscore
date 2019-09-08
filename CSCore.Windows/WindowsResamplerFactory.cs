using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCore.Windows
{
    internal class WindowsResamplerFactory : IResamplerFactory
    {
        public IWaveSource CreateResampler(IWaveSource waveSource, int targetSampleRate)
        {
            return new DSP.DmoResampler(waveSource, targetSampleRate);
        }

        public ISampleSource CreateResampler(ISampleSource sampleSource, int targetSampleRate)
        {
            return new DSP.DmoResampler(sampleSource.ToWaveSource(), targetSampleRate)
                .ToSampleSource();
        }
    }
}
