using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    /// <summary>
    /// Still in development
    /// </summary>
    public class AutoGain : GainSource
    {
        public AutoGain(IWaveStream source)
            : base(source)
        {
            Gain = 1f;
        }

        private float _storedgain = 1f;

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            float maxSample = 0;
            for (int i = offset; i < offset + count; i++)
            {
                if (Math.Abs(buffer[i]) > maxSample)
                    maxSample = Math.Abs(buffer[i]);
            }

            float newgain = 1f / maxSample - 0.2f;
            if (maxSample > 0.02f)
            {
                //if(newgain < _storedgain || newgain >= _storedgain + 0.1f)
                _storedgain = newgain;
                ApplyGain(buffer, offset, read, newgain);
            }
            return read;
        }
    }
}