using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class PanSource : SampleSourceBase
    {
        private float _pan = 0.0f;

        public float Pan
        {
            get
            {
                return _pan;
            }
            set
            {
                if (value < -1 || value > 1)
                    throw new ArgumentOutOfRangeException("value");
                _pan = value;
            }
        }

        public PanSource(IWaveStream source)
            : base(source)
        {
            if (source.WaveFormat.Channels != 2)
                throw new ArgumentException("Source has to be stereo.", "source");
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            if (read % 2 != 0)
                throw new InvalidOperationException("Read samples has to be a multiple of two.");

            float left = Math.Min(1, Pan + 1);
            float right = Math.Abs(Math.Max(-1, Pan - 1));

            for (int i = offset; i < offset + read; )
            {
                buffer[i++] *= left;
                buffer[i++] *= right;
            }

            return read;
        }
    }
}