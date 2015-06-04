using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    /// <summary>
    /// Provides data for the <see cref="SingleBlockNotificationStream.SingleBlockRead"/> event.
    /// </summary>
    public class SingleBlockReadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the sample of the left channel.
        /// </summary>
        public float Left { get; private set; }
        /// <summary>
        /// Gets the sample of the right channel.
        /// </summary>
        public float Right { get; private set; }

        /// <summary>
        /// Gets the samples of all channels if the number of <see cref="Channels"/> is greater or equal to three.
        /// </summary>
        /// <remarks>If the number of <see cref="Channels"/> is less than three, the value of the <see cref="Samples"/> property is null.</remarks>
        public float[] Samples { get; private set; }

        /// <summary>
        /// Gets the number of channels.
        /// </summary>
        public int Channels { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleBlockReadEventArgs"/> class.
        /// </summary>
        /// <param name="samples">The samples.</param>
        /// <param name="index">The index inside of the <paramref name="samples"/>-array.</param>
        /// <param name="channels">The number of channels.</param>
        public SingleBlockReadEventArgs(float[] samples, int index, int channels)
        {
            if (channels == 1)
                Left = samples[index];
            if (channels == 2)
            {
                Left = samples[index];
                Right = samples[index + 1];
            }
            if (channels > 3)
            {
                Left = samples[index];
                Right = samples[index + 1];
                Samples = new float[channels];
                for (int c = 0; c < channels; c++)
                {
                    Samples[c] = samples[index + c];
                }
            }
        }
    }
}
