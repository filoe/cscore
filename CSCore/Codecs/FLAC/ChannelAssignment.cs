namespace CSCore.Codecs.FLAC
{

    /// <summary>
    /// Defines the channel assignments.
    /// </summary>
    public enum ChannelAssignment
    {
        /// <summary>
        /// Independent assignment. 
        /// </summary>
        Independent = 0,
        /// <summary>
        /// Left/side stereo. Channel 0 becomes the left channel while channel 1 becomes the side channel.
        /// </summary>
        LeftSide = 1,
        /// <summary>
        /// Right/side stereo. Channel 0 becomes the right channel while channel 1 becomes the side channel.
        /// </summary>
        RightSide = 2,
        /// <summary>
        /// Mid/side stereo. Channel 0 becomes the mid channel while channel 1 becomes the side channel. 
        /// </summary>
        MidSide = 3,
    }
}