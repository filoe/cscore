using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    /// Internal parameter structure for the <see cref="DirectSoundFXFlanger"/> effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FlangerParameters
    {
        /// <summary>
        /// The wet dry mix.
        /// </summary>
        public float WetDryMix;
        /// <summary>
        /// The depth.
        /// </summary>
        public float Depth;
        /// <summary>
        /// The feedback.
        /// </summary>
        public float Feedback;
        /// <summary>
        /// The frequency.
        /// </summary>
        public float Frequency;
        /// <summary>
        /// The waveform.
        /// </summary>
        public int Waveform;
        /// <summary>
        /// The delay.
        /// </summary>
        public float Delay;
        /// <summary>
        /// The phase.
        /// </summary>
        public int Phase;
    }
}