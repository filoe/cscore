using CSCore.Streams;
using CSCore.Streams.SampleConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Visualization
{
    public class SampleDataProvider : ISampleSource
    {
        private ISampleSource _source;

        private Queue<float> _sampleBuffer;
        private Queue<float> _sampleBuffer1;

        public event EventHandler<BlockReadEventArgs> BlockRead;

        private object _lockObj = new object();

        private int _blockSize;

        public int BlockSize
        {
            get
            {
                return _blockSize;
            }
            set
            {
                lock (_lockObj)
                {
                    if (value < 1)
                        throw new ArgumentOutOfRangeException("value");
                    _blockSize = value;
                }
            }
        }

        private SampleDataProviderMode _mode = SampleDataProviderMode.Merge;

        public SampleDataProviderMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                Reset();
            }
        }

        public SampleDataProvider(IWaveStream source)
        {
            if (!(source is ISampleSource) && source is IWaveSource)
            {
                source = WaveToSampleBase.CreateConverter(source as IWaveSource);
            }
            else if (source is ISampleSource)
            {
            }
            else
            {
                throw new ArgumentException("source has to of type IWaveSource or ISampleSource");
            }

            _source = source as ISampleSource;
            BlockSize = (int)(source.WaveFormat.SampleRate * (40.0 / 1000.0));
            _sampleBuffer = new Queue<float>();
            _sampleBuffer1 = new Queue<float>();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            lock (_lockObj)
            {
                int read = _source.Read(buffer, offset, count);

                for (int n = 0; n < read; n += WaveFormat.Channels)
                {
                    if (WaveFormat.Channels > 1)
                    {
                        if (Mode != SampleDataProviderMode.Left && Mode != SampleDataProviderMode.Merge)
                            _sampleBuffer1.Enqueue(buffer[n + 1]);
                        else if (Mode == SampleDataProviderMode.Merge)
                            _sampleBuffer.Enqueue((buffer[n] + buffer[n + 1]) / 2f);
                    }
                    if (Mode != SampleDataProviderMode.Right && Mode != SampleDataProviderMode.Merge)
                    {
                        _sampleBuffer.Enqueue(buffer[n]);
                    }
                    if (_sampleBuffer.Count >= BlockSize || _sampleBuffer1.Count > BlockSize)
                    {
                        RaiseBlockRead();
                        Mode = Mode;
                    }
                }

                return read;
            }
        }

        public WaveFormat WaveFormat
        {
            get { return _source.WaveFormat; }
        }

        public long Position
        {
            get
            {
                return _source.Position;
            }
            set
            {
                _source.Position = value;
            }
        }

        public long Length
        {
            get { return _source.Length; }
        }

        public void Dispose()
        {
            _source.Dispose();
        }

        private void RaiseBlockRead()
        {
            float[] data = null;
            if (Mode != SampleDataProviderMode.Right)
            {
                data = new float[BlockSize];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = _sampleBuffer.Dequeue();
                }
            }

            float[] data1 = null;
            if (Mode != SampleDataProviderMode.Left && Mode != SampleDataProviderMode.Merge)
            {
                data1 = new float[BlockSize];
                for (int i = 0; i < data1.Length; i++)
                {
                    data1[i] = _sampleBuffer1.Dequeue();
                }
            }

            if (BlockRead != null)
            {
                BlockRead(this, new BlockReadEventArgs(data, data1));
            }
        }

        private void Reset()
        {
            _sampleBuffer.Clear();
            _sampleBuffer1.Clear();
        }
    }

    public enum SampleDataProviderMode
    {
        Left,
        Right,
        LeftAndRight,
        Merge
    }
}