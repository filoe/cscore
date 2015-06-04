using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Defines the characteristics of a <see cref="MFByteStream"/>.
    /// </summary>
    [Flags]
// ReSharper disable once InconsistentNaming
    public enum MFByteStreamCapsFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0x0,
        /// <summary>
        /// The byte stream can be read.
        /// </summary>
        IsReadable = 0x00000001,
        /// <summary>
        /// The byte stream can be written to.
        /// </summary>
        IsWriteable = 0x00000002,
        /// <summary>
        /// The byte stream can be seeked.
        /// </summary>
        IsSeekable = 0x00000004,
        /// <summary>
        /// The byte stream is from a remote source, such as a network.
        /// </summary>
        IsRemote = 0x00000008,
        /// <summary>
        /// The byte stream represents a file directory.
        /// </summary>
        IsDirectory = 0x00000080,
        /// <summary>
        /// Seeking within this stream might be slow. For example, the byte stream might download from a network.
        /// </summary>
        HasSlowSeek = 0x00000100,
        /// <summary>
        /// The byte stream is currently downloading data to a local cache. Read operations on the byte stream might take longer until the data is completely downloaded.This flag is cleared after all of the data has been downloaded.
        /// If the <see cref="HasSlowSeek"/> flag is also set, it means the byte stream must download the entire file sequentially. Otherwise, the byte stream can respond to seek requests by restarting the download from a new point in the stream.
        /// </summary>
        IsPartiallyDownloaded = 0x00000200,
        /// <summary>
        /// Another thread or process can open this byte stream for writing. If this flag is present, the length of the byte stream could change while it is being read.
        /// </summary>
        /// <remarks>Requires Windows 7 or later.</remarks>
        ShareWrite = 0x00000400,
        /// <summary>
        /// The byte stream is not currently using the network to receive the content. Networking hardware may enter a power saving state when this bit is set.
        /// </summary>
        /// <remarks>Requires Windows 8 or later.</remarks>
        DoesNotUseNetwork = 0x00000800
    }
}