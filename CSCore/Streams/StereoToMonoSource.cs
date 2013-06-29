using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams
{
    public class StereoToMonoSource : SampleSourceBase
    {
        WaveFormat _waveFormat;
        float[] _buffer;

        public StereoToMonoSource(IWaveStream source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if(source.WaveFormat.Channels != 2) 
                throw new ArgumentException("source has to have 2 channels", "source");

            _waveFormat = new WaveFormat(source.WaveFormat.SampleRate, 32, 1, AudioEncoding.IeeeFloat);
        }

        public unsafe override int Read(float[] buffer, int offset, int count)
        {
            _buffer = BufferUtils.CheckBuffer(buffer, count * 2);
            int read = _source.Read(_buffer, 0, count * 2);

            fixed (float* pbuffer = buffer)
            {
                float* ppbuffer = pbuffer + offset;
                for (int i = 0; i < read - 1; i+= 2)
                {
                    *(ppbuffer++) = (_buffer[i] + _buffer[i + 1]) / 2;
                }
            }

            return read / 2;
        }

        public override WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public override long Position
        {
            get
            {
                return _source.Position / 2;
            }
            set
            {
                _source.Position = value * 2;
            }
        }

        public override long Length
        {
            get { return _source.Length / 2; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _buffer = null;
        }
    }
}
