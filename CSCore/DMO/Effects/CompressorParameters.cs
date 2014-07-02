using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CompressorParameters
    {
        public float Gain;
        public float Attack;
        public float Release;
        public float Threshold;
        public float Ratio;
        public float Predelay;
    }
}