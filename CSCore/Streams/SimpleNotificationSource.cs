using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class SimpleNotificationSource : SampleSourceBase
    {

        /// <summary>
        /// Raised after every time the Read method gets called.
        /// </summary>
        public event EventHandler DataRead;
        /// <summary>
        /// Raised after the specific Interval
        /// </summary>
        public event EventHandler BlockRead;

        private int _blocksRead;
        private int _blockCount;

        /// <summary>
        /// Interval in blocks. One block equals on sample for each channel -> (channels *
        /// bitspersample) bits
        /// </summary>
        public int BlockCount
        {
            get
            {
                return _blockCount;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                _blockCount = value;
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

        public SimpleNotificationSource(IWaveStream source)
            : base(source)
        {
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            int channels = WaveFormat.Channels;

            if (BlockRead != null)
            {
                for (int i = 0; i < read / channels; i++)
                {
                    _blocksRead++;
                    if (_blocksRead >= BlockCount)
                    {
                        if (BlockRead != null)
                        {
                            BlockRead(this, EventArgs.Empty);
                        }
                        _blocksRead = 0;
                    }
                }
            }

            if (DataRead != null)
                DataRead(this, EventArgs.Empty);

            return read;
        }
    }
}