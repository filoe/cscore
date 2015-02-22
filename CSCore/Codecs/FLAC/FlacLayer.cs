namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Splits a flac file into a few basic layers and defines them. Mainly used for debugging purposes.
    /// </summary>
    public enum FlacLayer
    {
        /// <summary>
        /// Everything which is not part of a flac frame.
        /// </summary>
        /// <remarks>For example the "fLaC" sync code.</remarks>
        OutSideOfFrame,
        /// <summary>
        /// Everything metadata related.
        /// </summary>
        Metadata,
        /// <summary>
        /// Everything which is part of a frame but not part of its subframes.
        /// </summary>
        Frame,
        /// <summary>
        /// Everything subframe related.
        /// </summary>
        SubFrame
    }
}