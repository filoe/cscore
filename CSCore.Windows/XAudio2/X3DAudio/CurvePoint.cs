using System.Runtime.InteropServices;

namespace CSCore.XAudio2.X3DAudio
{
    /// <summary>
    /// Defines a DSP setting at a given normalized distance.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CurvePoint
    {
        /// <summary>
        /// Normalized distance. This must be within 0.0f to 1.0f.
        /// </summary>
        public float Distance;
        /// <summary>
        /// DSP control setting.
        /// </summary>
        public float DspSetting;
    }
}