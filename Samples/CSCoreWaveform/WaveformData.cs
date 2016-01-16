using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSCore;

namespace CSCoreWaveform
{
    public static class WaveformData
    {
        private const int NumberOfPoints = 2000;
        public static long Length;

        public static async Task<float[][]> GetData(IWaveSource waveSource)
        {
            if (waveSource == null)
                throw new ArgumentNullException("waveSource");

            return await Task.Run(() =>
            {
                var sampleSource = new InterruptDisposeChainSource(waveSource).ToSampleSource();

                var channels = sampleSource.WaveFormat.Channels;
                var blockSize = (int) (sampleSource.Length / channels / NumberOfPoints);
                var waveformDataChannels = new WaveformDataChannel[channels];
                for (var i = 0; i < channels; i++)
                {
                    waveformDataChannels[i] = new WaveformDataChannel(blockSize);
                }

                var buffer = new float[sampleSource.WaveFormat.BlockAlign * 5];
                var sampleCount = 0;

                var flag = true;
                while (flag)
                {
                    var samplesToRead = buffer.Length;
                    var read = sampleSource.Read(buffer, 0, samplesToRead);
                    for (var i = 0; i < read; i += channels)
                    {
                        for (var n = 0; n < channels; n++)
                        {
                            waveformDataChannels[n].AddSample(buffer[i + n]);
                            sampleCount++;
                        }
                    }

                    if (read == 0)
                        flag = false;
                }

                foreach (var waveformDataChannel in waveformDataChannels)
                {
                    waveformDataChannel.Finish();
                }

                Length = sampleCount;


                return waveformDataChannels.Select(x => x.GetData()).ToArray();
            });
        }

        private class InterruptDisposeChainSource : IWaveAggregator
        {
            private readonly IWaveSource _audioSource;

            public InterruptDisposeChainSource(IWaveSource audioSource)
            {
                _audioSource = audioSource;
            }

            public int Read(byte[] buffer, int offset, int count)
            {
                return _audioSource.Read(buffer, 0, count);
            }

            public void Dispose()
            {
                //do nothing
            }

            public bool CanSeek
            {
                get { return _audioSource.CanSeek; }
            }

            public WaveFormat WaveFormat
            {
                get { return _audioSource.WaveFormat; }
            }

            public long Position
            {
                get { return _audioSource.Position; }
                set { _audioSource.Position = value; }
            }

            public long Length
            {
                get { return _audioSource.Length; }
            }

            public IWaveSource BaseSource
            {
                get { return _audioSource; }
            }
        }

        private class WaveformDataChannel
        {
            private readonly int _blockSize;
            private readonly List<float> _maxData = new List<float>();

            private readonly List<float> _minData = new List<float>();
            private readonly SampleAnalyzer _sampleAnalyzer = new SampleAnalyzer();

            public WaveformDataChannel(int blockSize)
            {
                _blockSize = blockSize;
            }

            public void AddSample(float sample)
            {
                _sampleAnalyzer.AddSample(sample);
                if (_sampleAnalyzer.Counter >= _blockSize)
                {
                    _minData.Add(_sampleAnalyzer.AvgMin);
                    _maxData.Add(_sampleAnalyzer.AvgMax);

                    _sampleAnalyzer.Reset();
                }
            }

            public void Finish()
            {
                _minData.Add(_sampleAnalyzer.AvgMin);
                _minData.Add(_sampleAnalyzer.AvgMax);

                _sampleAnalyzer.Reset();
            }

            public float[] GetData()
            {
                _maxData.AddRange(_minData);
                var data = _maxData.ToArray();

                var z = 1 / data.Average(x => Math.Abs(x));
                z /= 2;
                for (var i = 0; i < data.Length; i++)
                {
                    data[i] *= z;
                    data[i] = Math.Min(1.5f, Math.Max(-1.5f, data[i]));
                }
                return data;
            }

            private class SampleAnalyzer
            {
                private readonly List<float> _neg = new List<float>();
                private readonly List<float> _pos = new List<float>();

                public float Min { get; private set; }

                public float Max { get; private set; }

                public float AvgMin
                {
                    get { return _neg.Any() ? _neg.Average() : 0; }
                }

                public float AvgMax
                {
                    get { return _pos.Any() ? _pos.Average() : 0; }
                }

                public int Counter { get; private set; }

                public void AddSample(float sample)
                {
                    Min = Math.Min(Min, sample);
                    Max = Math.Max(Max, sample);

                    if (sample < 0)
                        _neg.Add(sample);
                    else if (sample > 0)
                        _pos.Add(sample);

                    Counter++;
                }

                public void Reset()
                {
                    Counter = 0;
                    Min = 0;
                    Max = 0;
                    _neg.Clear();
                    _pos.Clear();
                }
            }
        }
    }
}