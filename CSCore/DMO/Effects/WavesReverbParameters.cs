using System.Runtime.InteropServices;

namespace CSCore.DMO.Effects
{
    /// <summary>
    /// Internal parameter structure for the <see cref="DirectSoundFXWavesReverb"/> effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WavesReverbParameters
    {
        /// <summary>
        /// The high freq rt ratio.
        /// </summary>
        public float HighFreqRTRatio;
        /// <summary>
        /// The in gain.
        /// </summary>
        public float InGain;
        /// <summary>
        /// The reverb mix.
        /// </summary>
        public float ReverbMix;
        /// <summary>
        /// The reverb time.
        /// </summary>
        public float ReverbTime;
    }
}