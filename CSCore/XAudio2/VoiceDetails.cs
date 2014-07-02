using System.Runtime.InteropServices;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     Contains information about the creation flags, input channels, and sample rate of a voice.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VoiceDetails
    {
        /// <summary>
        ///     Flags used to create the voice; see the individual voice interfaces for more information.
        /// </summary>
        public VoiceFlags CreationFlags;

        /// <summary>
        ///     Flags that are currently set on the voice.
        /// </summary>
        public int ActiveFlags;

        /// <summary>
        ///     The number of input channels the voice expects.
        /// </summary>
        public int InputChannels;

        /// <summary>
        ///     The input sample rate the voice expects.
        /// </summary>
        public int InputSampleRate;
    }
}