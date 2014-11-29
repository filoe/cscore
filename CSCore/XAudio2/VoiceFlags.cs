using System;

namespace CSCore.XAudio2
{
    /// <summary>
    ///     VoiceFlags
    /// </summary>
    [Flags]
    public enum VoiceFlags
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
        ///     <b>XAudio2.8 only:</b> Not supported on Windows.
        /// </summary>
        Music = 16,

        /// <summary>
        ///     <b>XAudio2.7 only:</b> Indicates that no samples were played.
        /// </summary>
        NoSamplesPlayed = 256
    }
}