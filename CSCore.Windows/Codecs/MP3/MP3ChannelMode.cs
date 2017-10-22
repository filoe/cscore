namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// Channelmode of MP3 data. For more information see the mp3 specification.
    /// </summary>
    public enum Mp3ChannelMode
    {
        /// <summary>
        /// Stereo (left and right).
        /// </summary>
        Stereo,
        /// <summary>
        /// Joint stereo.
        /// </summary>
        JointStereo,
        /// <summary>
        /// Dual channel.
        /// </summary>
        DualChannel,
        /// <summary>
        /// Mono (only one channel).
        /// </summary>
        Mono
    }
}
