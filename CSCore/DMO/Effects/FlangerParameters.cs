using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FlangerParameters
    {
        public float WetDryMix;
        public float Depth;
        public float Feedback;
        public float Frequency;
        public int Waveform;
        public float Delay;
        public int Phase;
    }
}