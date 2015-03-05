using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    /// Internal parameter structure for the <see cref="DirectSoundFXEcho"/> effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EchoParameters
    {
        /// <summary>
        /// The wet dry mix.
        /// </summary>
        public float WetDryMix;
        /// <summary>
        /// The feedback.
        /// </summary>
        public float Feedback;
        /// <summary>
        /// The left delay.
        /// </summary>
        public float LeftDelay;
        /// <summary>
        /// The right delay.
        /// </summary>
        public float RightDelay;
        /// <summary>
        /// The pan delay.
        /// </summary>
        public int PanDelay;
    }
}