namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// MP3 Format id.
    /// </summary>
    public enum Mp3FormatId : short
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Default value. Equals the MPEGLAYER3_ID_MPEG constant.
        /// </summary>
        Mpeg = 1,
        /// <summary>
        /// Constant frame size.
        /// </summary>
        ConstFrameSize = 2
    }
}