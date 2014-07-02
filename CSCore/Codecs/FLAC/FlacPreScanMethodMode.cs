namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Defines how to scan a flac stream.
    /// </summary>
    public enum FlacPreScanMethodMode
    {
        /// <summary>
        /// Don't scan the flac stream. This typically will cause a stream to be not seekable.
        /// </summary>
        None,

        /// <summary>
        /// Default value.
        /// </summary>
        Default,

        /// <summary>
        /// Scan synchronously.
        /// </summary>
        Sync,

        /// <summary>
        /// Scan async BUT don't use the stream while scan is running because the stream position
        /// will change while scanning. If you playback the stream, it will cause an error!
        /// </summary>
        Async
    }
}