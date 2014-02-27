using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class Equalizer : SampleSourceBase
    {
        public static Equalizer Create10BandEqualizer(IWaveStream source)
        {
            return new Equalizer(source) { SampleFilters = Create10BandEqFilter(source.WaveFormat.SampleRate, source.WaveFormat.Channels) };
        }

        public static EqFilterCollection Create10BandEqFilter(int sampleRate, int channelCount, float bandWidth = 18, float defaultGain = 0)
        {
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

            EqFilterCollection filterCollection = new EqFilterCollection(channelCount);
            foreach (var filter in sampleFilters)
            {
                filterCollection.Add(filter);
            }

            return filterCollection;
        }

        private IList<EqFilterEntry> _sampleFilters;
        public IList<EqFilterEntry> SampleFilters
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

            for (int n = offset; n < count; n++)
            {
                buffer[n] = Math.Max(-1, Math.Min(buffer[n], 1));
            }

            return read;
        }
    }
}
