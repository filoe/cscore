using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class PeakEventArgs : EventArgs
    {
        public float[] ChannelPeakValues { get; private set; }
        public float PeakValue { get; private set; }

        public PeakEventArgs(float[] channelPeakValues, float peakValue)
        {
            if (channelPeakValues == null || channelPeakValues.Length == 0)
                throw new ArgumentException("channelPeakValues");

            ChannelPeakValues = channelPeakValues;
            PeakValue = peakValue;
        }
    }
}
