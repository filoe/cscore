using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSCore.Streams
{
    public class VolumeSource : SampleSourceBase
    {
        float _volume = 1f;
        public virtual float Volume
        {
            get { return _volume; }
            set
            {
                if (_volume < 0 || _volume > 1)
                    throw new ArgumentOutOfRangeException("value");
                _volume = value;
            }
        }

        public VolumeSource(IWaveStream source)
            : base(source)
        {
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            for (int i = offset; i < read + offset; i++)
            {
                buffer[i] *= Volume;
            }

            return read;
        }
    }
}
