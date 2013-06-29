using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class NotificationSource : SampleSourceBase
    {
        public event EventHandler<BlockReadEventArgs<float>> BlockRead;

        Queue<float> _buffer;

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

        public NotificationSource(IWaveStream source)
            : base(source)
        {
            BlockSize = (int)(source.WaveFormat.SampleRate * (40.0 / 1000.0));
            _buffer = new Queue<float>(BlockSize);
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            for (int i = 0; i < read;)
            {
                for (int n = 0; n < WaveFormat.Channels; n++ )
                {
                    _buffer.Enqueue(buffer[i++]);
                }
                if (_buffer.Count >= BlockSize * WaveFormat.Channels)
                {
                    if (BlockRead != null)
                    {
                        float[] b = new float[BlockSize * WaveFormat.Channels];
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
