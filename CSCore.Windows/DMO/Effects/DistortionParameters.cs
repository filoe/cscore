using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    /// Internal parameter structure for the <see cref="DirectSoundFXDistortion"/> effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DistortionParameters
    {
        /// <summary>
        /// The gain.
        /// </summary>
        public float Gain;
        /// <summary>
        /// The edge.
        /// </summary>
        public float Edge;
        /// <summary>
        /// The post eq center frequency.
        /// </summary>
        public float PostEQCenterFrequency;
        /// <summary>
        /// The post eq bandwidth.
        /// </summary>
        public float PostEQBandwidth;
        /// <summary>
        /// The pre lowpass cutoff.
        /// </summary>
        public float PreLowpassCutoff;
    }
}