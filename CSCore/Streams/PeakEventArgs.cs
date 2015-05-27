using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    /// <summary>
    /// Provides data for the <see cref="PeakMeter.PeakCalculated"/> event.
    /// </summary>
    public class PeakEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the individual peak value for each channel.
        /// </summary>
        public float[] ChannelPeakValues { get; private set; }
        /// <summary>
        /// Gets the master peak value.
        /// </summary>
        public float PeakValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeakEventArgs"/> class.
        /// </summary>
        /// <param name="channelPeakValues">The channel peak values.</param>
        /// <param name="peakValue">The master peak value.</param>
        /// <exception cref="System.ArgumentException"><paramref name="channelPeakValues"/> is null or empty.</exception>
        public PeakEventArgs(float[] channelPeakValues, float peakValue)
        {
            if (channelPeakValues == null || channelPeakValues.Length == 0)
                throw new ArgumentException("channelPeakValues");

            ChannelPeakValues = channelPeakValues;
            PeakValue = peakValue;
        }
    }
}
