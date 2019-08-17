using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Linux.DSP
{
    public class Resampler : IWaveSource
    {
        public Resampler(IWaveSource source, int destinationSampleRate)
        {
            throw new NotImplementedException();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool CanSeek { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public long Position { get; set; }
        public long Length { get; private set; }
    }
}
