using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DistortionParameters
    {
        public float Gain;
        public float Edge;
        public float PostEQCenterFrequency;
        public float PostEQBandwidth;
        public float PreLowpassCutoff;
    }
}