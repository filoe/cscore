using System;

namespace CSCore.DSP
{
    public class PeakCalculatedEventArgs : EventArgs
    {
        public float MaxLeftPeak { get; private set; }
        public float MaxRightPeak { get; private set; }

        public float RMSLeft { get; private set; }
        public float RMSRight { get; private set; }

        public PeakCalculatedEventArgs(float maxLeft, float maxRight, float rmsleft, float rmsright)
        {
            MaxLeftPeak = maxLeft;
            MaxRightPeak = maxRight;
            RMSLeft = rmsleft;
            RMSRight = rmsright;
        }
    }
}