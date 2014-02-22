using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class PeakMeter : SampleSourceBase
    {
        /// <summary>
        /// Average value of all ChannelPeakValues.
        /// </summary>
        public float PeakValue
        {
            get
            {
                return ChannelPeakValues.Average();
            }
        }
        public float[] ChannelPeakValues { get; private set; }
        public int BlocksToProcess { get; set; }

        public event EventHandler<PeakEventArgs> PeakCalculated;

        private int _blocksProcessed = 0;

        public PeakMeter(IWaveStream source)
            : base(source)
        {
            ChannelPeakValues = new float[WaveFormat.Channels];
            BlocksToProcess = source.WaveFormat.SampleRate / 4;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);

            int channels = WaveFormat.Channels;
            for (int i = offset; i < read; i++)
            {
                ChannelPeakValues[i] = Math.Max(ChannelPeakValues[i], Math.Abs(buffer[i]));
                if(++_blocksProcessed == BlocksToProcess)
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
