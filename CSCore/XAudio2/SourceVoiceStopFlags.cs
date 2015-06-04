namespace CSCore.XAudio2
{
    /// <summary>
    ///     Flags that specify how a <see cref="XAudio2SourceVoice" /> is stopped.
    /// </summary>
    public enum SourceVoiceStopFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0x0000,

        /// <summary>
        ///     Continue emitting effect output after the voice is stopped.
        /// </summary>
        PlayTails = 0x0020
    }
}