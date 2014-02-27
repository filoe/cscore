using CSCore.Utils;
using System;

namespace CSCore.DSP
{
    public class FFTAggregator : WaveAggregatorBase
    {
        public event EventHandler<FFTCalculatedEventArgs> FFTCalculated;

        private int _iteratorOffset = 0;

        public FFTAggregator()
            : base()
        {
        }

        public FFTAggregator(IWaveSource baseSource)
            : base(baseSource)
        {
        }

        public FFTAggregator(IWaveSource baseSource, int bands)
            : base(baseSource)
        {
            BandCount = bands;
        }

        private int _bandCount = 1024;

        public int BandCount
        {
            get
            {
                return _bandCount;
            }
            set
            {
                if (CSMath.GetExponent(value, 2) % 1 != 0.0)
                    throw new ArgumentException("BandCount has to be a value of bands(x) = 2^x");
                _bandCount = value;
            }
        }

        private Complex[] _complex;

        protected Complex[] Complex
        {
            get { return _complex ?? (_complex = new Complex[_bandCount]); }
            set { _complex = value; }
        }

        public override unsafe int Read(byte[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            fixed (byte* pbuffer = buffer)
            {
                byte* ppbuffer = pbuffer;
                for (int i = 0; i < read / WaveFormat.BytesPerSample; i += WaveFormat.Channels)
                {
                    ppbuffer = pbuffer + (i + 1) * WaveFormat.BytesPerSample;
                    float sample = ConvertToSample(ppbuffer, WaveFormat.BitsPerSample, false, true);

                    Complex[_iteratorOffset++].Real = (float)(sample * FastFourierTransformation.HammingWindow(_iteratorOffset - 1, _bandCount));
                    if (_iteratorOffset >= BandCount)
                    {
                        RaiseFFTCalculated(Complex);
                        Reset();
                    }
                }
            }
            return read;
        }

        protected virtual void RaiseFFTCalculated(Complex[] complex)
        {
            FastFourierTransformation.FFT1(complex, (int)Math.Log(_bandCount, 2.0), FFTMode.Forward);
            if (FFTCalculated != null)
                FFTCalculated(this, new FFTCalculatedEventArgs(complex));
        }

        protected virtual void Reset()
        {
            _iteratorOffset = 0;
            /*if (Complex != null && Complex.Length == Bands)
            {
                for (int i = 0; i < Complex.Length; i++)
                    Complex[i] = Utils.Complex.Zero;
            }
            else
                Complex = null;
            */
            Complex = null;
        }

        private unsafe float ConvertToSample(byte* buffer, int bitsPerSample, bool autoIncrementPtr = true, bool mindown = true)
        {
            float value;
            if (bitsPerSample == 8)
                value = CSMath.Bit8ToFloat(buffer, mindown);
            else if (bitsPerSample == 16)
                value = CSMath.Bit16ToFloat(buffer, mindown);
            else if (bitsPerSample == 24)
                value = CSMath.Bit24ToFloat(buffer, mindown);
            else if (bitsPerSample == 32)
                value = CSMath.Bit32ToFloat(buffer, mindown);
            else
                throw new ArgumentOutOfRangeException("bitsPerSample");

            if (autoIncrementPtr)
                buffer += (bitsPerSample / 8);

            return value;
        }
    }
}