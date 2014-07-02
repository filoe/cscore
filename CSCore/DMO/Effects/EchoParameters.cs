using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EchoParameters
    {
        public float WetDryMix;
        public float Feedback;
        public float LeftDelay;
        public float RightDelay;
        public int PanDelay;
    }
}