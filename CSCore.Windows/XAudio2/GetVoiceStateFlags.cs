namespace CSCore.XAudio2
{
    /// <summary>
    ///     Flags controlling which voice state data should be returned.
    /// </summary>
    public enum GetVoiceStateFlags
    {
        /// <summary>
        ///     Calculate all values.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Calculate all values except <see cref="VoiceState.SamplesPlayed" />.
        /// </summary>
        NoSamplesPlayed = 0x0100
    }
}