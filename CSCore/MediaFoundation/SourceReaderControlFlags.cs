using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Defines flags for the <see cref="MFSourceReader.ReadSample"/> method.
    /// </summary>
    [Flags]
    public enum SourceReaderControlFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0x00000000,
        /// <summary>
        /// Retrieve any pending samples, but do not request any more samples from the media source. To get all of the pending samples, call <see cref="MFSourceReader.ReadSample"/> with this flag until the method returns a NULL media sample pointer.
        /// </summary>
        Drain = 0x00000001
    }
}