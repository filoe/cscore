using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     VoiceFlags
    /// </summary>
    [Flags]
    public enum VoiceFlags : uint
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0,

        /// <summary>
        ///     No pitch control is available on the voice.
        /// </summary>
        NoPitch = 2,

        /// <summary>
        ///     No sample rate conversion is available on the voice. The voice's outputs must have the same sample rate.
        /// </summary>
        NoSampleRateConversition = 4,

        /// <summary>
        ///     The filter effect should be available on this voice.
        /// </summary>
        UseFilter = 8,

        /// <summary>
        ///     Not supported on Windows.
        /// </summary>
        Music = 16
    }
}