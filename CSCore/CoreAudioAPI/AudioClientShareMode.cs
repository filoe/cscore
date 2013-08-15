namespace CSCore.CoreAudioAPI
{
    /// <summary>
    /// AudioClient share mode
    /// </summary>
    public enum AudioClientShareMode
    {
        /// <summary>
        /// The device will be opened in shared mode and use the WAS format.
        /// </summary>
        Shared,

        /// <summary>
        /// The device will be opened in exclusive mode and use the application specified format.
        /// </summary>
        Exclusive
    }
}