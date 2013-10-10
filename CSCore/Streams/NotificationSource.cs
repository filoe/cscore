using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class NotificationSource : SampleSourceBase
    {
        public event EventHandler<BlockReadEventArgs<float>> BlockRead;

        private Queue<float> _buffer;

        private int _blockSize;

        /// <summary>
        /// Interval in blocks. One block equals on sample for each channel -> (channels *
        /// bitspersample) bits
        /// </summary>
        public int BlockCount
        {
            get
            {
                return _blockSize;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                _blockSize = value;
            }
        }

        /// <summary>
        /// Interval in milliseconds.
        /// </summary>
        public int Interval
        {
            get
            {
                return (int)(1000.0 * ((double)BlockCount / (double)WaveFormat.SampleRate));
            }
            set
            {
                int v = (int)(((double)(value * WaveFormat.SampleRate)) / 1000.0);
                v = Math.Max(1, v);
                BlockCount = v;
            }
        }

        public NotificationSource(IWaveStream source)
            : base(source)
        {
            BlockCount = (int)(source.WaveFormat.SampleRate * (40.0 / 1000.0));
            _buffer = new Queue<float>(BlockCount * source.WaveFormat.Channels);
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            int channels = WaveFormat.Channels;

            for (int i = offset; i < offset + read; )
            {
                for (int n = 0; n < channels; n++)
                {
                    _buffer.Enqueue(buffer[i++]);
                }
                if (_buffer.Count >= BlockCount * WaveFormat.Channels)
                {
                    if (BlockRead != null)
                    {
                        float[] b = new float[BlockCount * WaveFormat.Channels];
                        for (int n = 0; n < b.Length; n++)
                            b[n] = _buffer.Dequeue();

                        BlockRead(this, new BlockReadEventArgs<float>(b, b.Length));

                        b = null;
                    }
                }
            }

            return read;
        }
    }
}