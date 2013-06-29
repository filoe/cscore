using CSCore.Utils;
using System;

namespace CSCore.DSP
{
    public class FFTAggregator : WaveAggregatorBase
    {
        public event EventHandler<FFTCalculatedEventArgs> FFTCalculated;
        int _iteratorOffset = 0;

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
            Bands = bands;
        }

        int _bands = 1024;
        public int Bands
        {
            get { return _bands; }
            set
            {
                if (CSMath.GetExponent(value, 2) % 1 != 0)
                    throw new ArgumentException("Bands has to be a value of bands(x) = 2^x");
                _bands = value;
            }
        }

        Complex[] _complex;
        protected Complex[] Complex
        {
            get { return _complex ?? (_complex = new Complex[_bands]); }
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
                    float sample = Utils.Utils.ConvertToSample(ppbuffer, WaveFormat.BitsPerSample, false, true);

                    Complex[_iteratorOffset++].Real = sample * FastFourierTransformation.HammingWindow(_iteratorOffset - 1, _bands);
                    if (_iteratorOffset >= Bands)
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
            FastFourierTransformation.DoFFT(complex, _bands, true);
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
    }
}
