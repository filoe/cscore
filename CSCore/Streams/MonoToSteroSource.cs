using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class MonoToStereoSource : SampleSourceBase
    {
        private WaveFormat _waveFormat;
        private float[] _buffer;

        public override WaveFormat WaveFormat
        {
            get
            {
                return _waveFormat;
            }
        }

        public MonoToStereoSource(IWaveStream source)
            : base(source)
        {
            if (source.WaveFormat.Channels != 1)
                throw new ArgumentException("format of source has to be stereo(1 channel)", "source");
            _waveFormat = new WaveFormat(source.WaveFormat.SampleRate, 32, 2, AudioEncoding.IeeeFloat);
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int sourceCount = count / 2;
            _buffer = _buffer.CheckBuffer(sourceCount);

            int bi = offset;
            int read = _source.Read(_buffer, 0, sourceCount);
            for (int i = 0; i < read; i++)
            {
                buffer[bi++] = _buffer[i];
                buffer[bi++] = _buffer[i];
            }

            return read * 2;
        }

        public override long Position
        {
            get
            {
                return base.Position * 2;
            }
            set
            {
                base.Position = value / 2;
            }
        }

        public override long Length
        {
            get
            {
                return base.Length * 2;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _buffer = null;
        }
    }
}