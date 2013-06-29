using System;

namespace CSCore.DSP
{
    [Obsolete()]
    public class AdjustableResampler : DmoResampler
    {
        int _sampleRate;

        public AdjustableResampler(IWaveSource source)
            : this(source, source.WaveFormat.SampleRate)
        {
        }

        public AdjustableResampler(IWaveSource source, int sampleRate)
            : base(source, sampleRate)
        {
            _sampleRate = sampleRate;
        }

        public int SampleRate
        {
            get { return _sampleRate; }
            set { SetSampleRate(value); }
        }

        private void SetSampleRate(int sampleRate)
        {
            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");

            if (sampleRate == _sampleRate)
                return;
            _sampleRate = sampleRate;

            lock (_lockObj)
            {
                var format = new WaveFormat(_outputformat, sampleRate);
                nativeObject.SetOutputType(0, format);
                _ratio = (double)BaseStream.WaveFormat.BytesPerSecond / (double)format.BytesPerSecond;
            }
        }
    }
}
