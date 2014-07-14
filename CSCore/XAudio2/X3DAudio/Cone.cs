using System;
using System.Runtime.InteropServices;

namespace CSCore.XAudio2.X3DAudio
{
    /// <summary>
    /// Specifies directionality for a single-channel non-Low-Frequency-Effect emitter by scaling DSP behavior with respect to the emitter's orientation.
    /// </summary>
    /// <remarks>
    /// For a detailed explanation of sound cones see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ee418803(v=vs.85).aspx"/>.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Cone
    {
        /// <summary>
        /// X3DAUDIO_2PI
        /// </summary>
        public const double X3DAUDIO_2PI = Math.PI * 2;

        /// <summary>
        /// Inner cone angle in radians. This value must be within 0.0f to <see cref="X3DAUDIO_2PI"/>.
        /// </summary>
        public float InnerAngle;
        /// <summary>
        /// Outer cone angle in radians. This value must be within InnerAngle to <see cref="X3DAUDIO_2PI"/>.
        /// </summary>
        public float OuterAngle;
        /// <summary>
        /// Volume scaler on/within inner cone. This value must be within 0.0f to 2.0f.
        /// </summary>
        public float InnerVolume;
        /// <summary>
        /// Volume scaler on/beyond outer cone. This value must be within 0.0f to 2.0f.
        /// </summary>
        public float OuterVolume;
        /// <summary>
        /// LPF direct-path or reverb-path coefficient scaler on/within inner cone. This value is only used for LPF calculations and must be within 0.0f to 1.0f.
        /// </summary>
        public float InnerLPF;
        /// <summary>
        /// LPF direct-path or reverb-path coefficient scaler on or beyond outer cone. This value is only used for LPF calculations and must be within 0.0f to 1.0f.
        /// </summary>
        public float OuterLPF;
        /// <summary>
        /// Reverb send level scaler on or within inner cone. This must be within 0.0f to 2.0f.
        /// </summary>
        public float InnerReverb;
        /// <summary>
        /// Reverb send level scaler on/beyond outer cone. This must be within 0.0f to 2.0f.
        /// </summary>
        public float OuterReverb;
    }
}