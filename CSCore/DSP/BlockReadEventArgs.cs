using System;

namespace CSCore.DSP
{
    public class PeakCalculatedEventArgs : EventArgs
    {
        public float MaxLeftPeak { get; private set; }
        public float MaxRightPeak { get; private set; }

        public PeakCalculatedEventArgs(float maxLeft, float maxRight)
        {
            MaxLeftPeak = maxLeft;
            MaxRightPeak = maxRight;
        }
    }
}
