namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Specifies the origin for a seek request.
    /// </summary>
// ReSharper disable once InconsistentNaming
    public enum MFByteStreamSeekOrigin
    {
        /// <summary>
        /// The seek position is specified relative to the start of the stream.
        /// </summary>
        Begin = 0,
        /// <summary>
        /// The seek position is specified relative to the current read/write position in the stream.
        /// </summary>
        Current = Begin + 1
    }
}