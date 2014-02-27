using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class EqFilter : ICloneable
    {
        private BiQuad _filter;
        private int _sampleRate;
        private double _centerFrequency;
        private  float _bandWidth;
        private float _gain;

        public float GainDB
        {
            get { return _gain; }
            set
            {
                _gain = value;
                _filter.SetPeakGain(value);
            }
        }

        public float BandWidth
        {
            get { return _bandWidth; }
            set
            {
                _bandWidth = value;
                _filter.SetQ(value);
            }
        }

        public double Frequency
        {
            get { return _centerFrequency; }
            private set
            {
                _centerFrequency = value;
                _filter.SetFrequency(value, _sampleRate);
            }
        }

        public int SampleRate
        {
            get { return _sampleRate; }
            set
            {
                _sampleRate = value;
                _filter.SetFrequency(_centerFrequency, value);
            }
        }


        public EqFilter(int sampleRate, double centerFrequency, float bandWidth, float gain)
        {
            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");
            if (centerFrequency <= 0)
                throw new ArgumentOutOfRangeException("centerFrequency");
            if (bandWidth <= 0)
                throw new ArgumentOutOfRangeException("bandWidth");

            _sampleRate = sampleRate;
            _centerFrequency = centerFrequency;
            _bandWidth = bandWidth;
            _gain = gain;

            _filter = BiQuad.CreatePeakEQFilter(sampleRate, centerFrequency, bandWidth, gain);
        }

        public void Process(float[] input, int offset, int count, int channelIndex, int channelCount)
        {
            for (int i = channelIndex + offset; i < count + offset; i += channelCount)
            {
                input[i] = _filter.Process(input[i]);
            }
        }

        public object Clone()
        {
            return new EqFilter(SampleRate, Frequency, BandWidth, GainDB);
        }
    }
}
