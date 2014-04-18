using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    /// <summary>
    /// Peakmeter.
    /// </summary>
    public class PeakMeter : SampleSourceBase
    {
        /// <summary>
        /// Gets the average value of all <see cref="ChannelPeakValues"/>.
        /// </summary>
        public float PeakValue
        {
            get
            {
                return ChannelPeakValues.Average();
            }
        }

        /// <summary>
        /// Gets the peak values for all channels. 
        /// </summary>
        public float[] ChannelPeakValues { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int BlocksToProcess { get; set; }

        /// <summary>
        /// Event which gets raised when a new peak value is available. 
        /// </summary>
        public event EventHandler<PeakEventArgs> PeakCalculated;

        private int _blocksProcessed = 0;

        /// <summary>
        /// Creates a new instance of the <see cref="PeakMeter"/> class.
        /// </summary>
        /// <param name="source"></param>
        public PeakMeter(IWaveStream source)
            : base(source)
        {
            ChannelPeakValues = new float[WaveFormat.Channels];
            BlocksToProcess = source.WaveFormat.SampleRate / 4;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            //todo: throw exception or subtract it?
            if (count % WaveFormat.BlockAlign != 0)
                throw new ArgumentOutOfRangeException("count");
            if (offset % WaveFormat.BlockAlign != 0)
                throw new ArgumentOutOfRangeException("offset");

            int read = base.Read(buffer, offset, count);

            int channels = WaveFormat.Channels;
            for (int i = offset; i < read; i++)
            {
                int channel = i % channels;
                ChannelPeakValues[channel] = Math.Max(ChannelPeakValues[channel], Math.Abs(buffer[i]));

                if (channel == channels - 1)
                    _blocksProcessed++;
                if(_blocksProcessed == BlocksToProcess)
                {
                    RaisePeakCalculated();
                    Reset();
                }
            }

            return read;
        }

        /// <summary>
        /// Sets all ChannelPeakValues to zero and resets the amount of processed blocks.
        /// </summary>
        public void Reset()
        {
            Array.Clear(ChannelPeakValues, 0, ChannelPeakValues.Length);
            _blocksProcessed = 0;
        }

        private void RaisePeakCalculated()
        {
            if (PeakCalculated != null)
                PeakCalculated(this, new PeakEventArgs(ChannelPeakValues, PeakValue));
        }
    }
}
