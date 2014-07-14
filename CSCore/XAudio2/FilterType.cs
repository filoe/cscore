namespace CSCore.XAudio2
{
    /// <summary>
    ///     Indicates the filter type.
    /// </summary>
    /// <remarks>
    ///     Note  Note that the DirectX SDK versions of XAUDIO2 do not support the LowPassOnePoleFilter or the
    ///     HighPassOnePoleFilter.
    /// </remarks>
    public enum FilterType
    {
        /// <summary>
        ///     Attenuates (reduces) frequencies above the cutoff frequency.
        /// </summary>
        LowPassFilter = 0,

        /// <summary>
        ///     Attenuates frequencies outside a given range.
        /// </summary>
        BandPassFilter = 1,

        /// <summary>
        ///     Attenuates frequencies below the cutoff frequency.
        /// </summary>
        HighPassFilter = 2,

        /// <summary>
        ///     Attenuates frequencies inside a given range.
        /// </summary>
        NotchFilter = 3,

        /// <summary>
        ///     <b>XAudio2.8 only:</b> Attenuates frequencies above the cutoff frequency. This is a one-pole filter, and
        ///     <see cref="FilterParameters.OneOverQ" /> has no effect.
        /// </summary>
        LowPassOnePoleFilter = 4,

        /// <summary>
        ///     <b>XAudio2.8 only:</b> Attenuates frequencies below the cutoff frequency. This is a one-pole filter, and
        ///     <see cref="FilterParameters.OneOverQ" /> has no effect.
        /// </summary>
        HighPassOnePoleFilter = 5
    }
}