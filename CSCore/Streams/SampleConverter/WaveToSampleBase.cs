using System;

namespace CSCore.Streams.SampleConverter
{
    public abstract class WaveToSampleBase : ISampleSource
    {
        public static ISampleSource CreateConverter(IWaveSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int bps = source.WaveFormat.BitsPerSample;
            if (source.WaveFormat.IsPCM())
            {
                switch (bps)
                {
                    case 8:
                        return new Pcm8BitToSample(source);

                    case 16:
                        return new Pcm16BitToSample(source);

                    case 24:
                        return new Pcm24BitToSample(source);

                    default:
                        throw new NotSupportedException("Waveformat is not supported. Invalid BitsPerSample value.");
                }
            }
            else if (source.WaveFormat.IsIeeeFloat() && bps == 32)
            {
                return new IeeeFloatToSample(source);
            }
            else
            {
                throw new NotSupportedException("Waveformat is not supported. Invalid WaveformatTag.");
            }
        }

        [ThreadStatic]
        protected byte[] _buffer;

        protected IWaveSource _source;
        protected double _bpsratio;
        private WaveFormat _waveFormat;

        public WaveToSampleBase(IWaveSource source, int bits, AudioEncoding encoding)
        {
            if (source == null) throw new ArgumentNullException("source");

            _source = source;
            _waveFormat = new WaveFormat(source.WaveFormat.SampleRate, 32,
                                source.WaveFormat.Channels, AudioEncoding.IeeeFloat);
            _bpsratio = 32.0 / bits;
        }

        public abstract int Read(float[] buffer, int offset, int count);

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public long Position
        {
            get
            {
                return (long)(_source.Position / _bpsratio);
            }
            set
            {
                _source.Position = (long)(value * _bpsratio);
            }
        }

        public long Length
        {
            get { return (long)(_source.Length / _bpsratio); }
        }

        public virtual void Dispose()
        {
            _source.Dispose();
        }
    }
}