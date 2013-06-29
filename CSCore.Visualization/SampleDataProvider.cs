using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.Streams;
using CSCore.Streams.SampleConverter;

namespace CSCore.Visualization
{
    public class SampleDataProvider : ISampleSource
    {
        ISampleSource _source;

        Queue<float> _sampleBuffer;

        public event EventHandler<BlockReadEventArgs> BlockRead;

        int _blockSize;
        public int BlockSize
        {
            get { return _blockSize; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                _blockSize = value;
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
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int read = _source.Read(buffer, offset, count);

            for (int n = 0; n < read; n += WaveFormat.Channels)
            {
                _sampleBuffer.Enqueue(buffer[n]);
                if (_sampleBuffer.Count >= BlockSize)
                {
                    RaiseBlockRead();
                }
            }

            return read;
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
            float[] data = new float[BlockSize];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = _sampleBuffer.Dequeue();
            }

            if (BlockRead != null)
            {
                BlockRead(this, new BlockReadEventArgs(data));
            }
        }
    }
}
