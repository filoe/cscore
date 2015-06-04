using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    /// Internal parameter structure for the <see cref="DirectSoundFXCompressor"/> effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CompressorParameters
    {
        /// <summary>
        /// The gain.
        /// </summary>
        public float Gain;
        /// <summary>
        /// The attack.
        /// </summary>
        public float Attack;
        /// <summary>
        /// The release.
        /// </summary>
        public float Release;
        /// <summary>
        /// The threshold.
        /// </summary>
        public float Threshold;
        /// <summary>
        /// The ratio.
        /// </summary>
        public float Ratio;
        /// <summary>
        /// The predelay.
        /// </summary>
        public float Predelay;
    }
}