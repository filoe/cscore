using System;

namespace CSCore.DSP
{
    [Obsolete()]
    public class AdjustableResampler : DmoResampler
    {
        private int _sampleRate;
        private readonly int _startSampleRate;

        public AdjustableResampler(IWaveSource source)
            : this(source, source.WaveFormat.SampleRate)
        {
        }

        public AdjustableResampler(IWaveSource source, int sampleRate)
            : base(source, sampleRate)
        {
            _sampleRate = sampleRate;
            _startSampleRate = sampleRate;
        }

        public int SampleRate
        {
            get { return _sampleRate; }
            set { SetSampleRate(value); }
        }

        public void Reset()
        {

            SampleRate = _startSampleRate;
        }

        private void SetSampleRate(int sampleRate)
        {
            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException("_sampleRate");

            if (sampleRate == _sampleRate)
                return;
            _sampleRate = sampleRate;

            lock (_lockObj)
            {
                var format = new WaveFormat(_outputformat, sampleRate);
                _resampler.MediaObject.SetOutputType(0, format);
                _ratio = (double)BaseStream.WaveFormat.BytesPerSecond / (double)format.BytesPerSecond;
            }
        }
    }
}