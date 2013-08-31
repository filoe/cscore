using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    //todo: add channel support
    public class Equalizer : SampleSourceBase
    {
        public static Equalizer Create11BandEqualizer(IWaveStream source)
        {
            int sampleRate = source.WaveFormat.SampleRate;
            float bandWidth = 18;
            float defaultGain = 0;
            var sampleFilters = new EqFilter[] 
            {
                new EqFilter(sampleRate, 31, bandWidth, defaultGain),
                new EqFilter(sampleRate, 62, bandWidth, defaultGain),
                new EqFilter(sampleRate, 125, bandWidth, defaultGain),
                new EqFilter(sampleRate, 250, bandWidth, defaultGain),
                new EqFilter(sampleRate, 500, bandWidth, defaultGain),
                new EqFilter(sampleRate, 1000, bandWidth, defaultGain),
                new EqFilter(sampleRate, 2000, bandWidth, defaultGain),
                new EqFilter(sampleRate, 4000, bandWidth, defaultGain),
                new EqFilter(sampleRate, 8000, bandWidth, defaultGain),
                new EqFilter(sampleRate, 16000, bandWidth, defaultGain)
            };

            var e = new Equalizer(source);
            foreach (var c in sampleFilters)
            {
                e.SampleFilters.Add(c);
            }
            return e;
        }

        private EqFilterCollection _sampleFilters;
        public EqFilterCollection SampleFilters
        {
            get { return _sampleFilters; }
            set { _sampleFilters = value; }
        }

        public Equalizer(IWaveStream source)
            : base(source)
        {
            _sampleFilters = new EqFilterCollection(source.WaveFormat.Channels);
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            for (int c = 0; c < WaveFormat.Channels; c++)
            {
                for (int i = _sampleFilters.Count; i-- > 0; )
                {
                    _sampleFilters[i].Filters[c].Process(buffer, offset, read, c, WaveFormat.Channels);
                }
            }

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Math.Max(-1, Math.Min(buffer[i], 1));
            }

            return read;
        }
    }
}
