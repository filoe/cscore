using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class SingleBlockReadEventArgs : EventArgs
    {
        public float Left { get; private set; }
        public float Right { get; private set; }

        /// <summary>
        /// Do not use this in combination with mono or stereo
        /// </summary>
        public float[] Samples { get; private set; }

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
