using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Defines the characteristics of a media source.
    /// </summary>
    /// <remarks>For more information, see <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms694277(v=vs.85).aspx"/>.</remarks>
    [Flags]
// ReSharper disable once InconsistentNaming
    public enum MFMediaSourceCharacteristics
    {
        /// <summary>
        /// This flag indicates a data source that runs constantly, such as a live presentation. If the source is stopped and then restarted, there will be a gap in the content.
        /// </summary>
        IsLive = 0x1,
        /// <summary>
        /// The media source supports seeking.
        /// </summary>
        CanSeek = 0x2,
        /// <summary>
        /// The source can pause.
        /// </summary>
        CanPause = 0x4,
        /// <summary>
        /// The media source downloads content. It might take a long time to seek to parts of the content that have not been downloaded.
        /// </summary>
        HasSlowSeek = 0x8,
        /// <summary>
        /// The media source delivers a playlist, which might contain more than one entry.
        /// </summary>
        /// <remarks>Requires Windows 7 or later.</remarks>
        HasMultiplePresentations = 0x10,
        /// <summary>
        /// The media source can skip forward in the playlist. Applies only if the <see cref="HasMultiplePresentations"/> flag is present.
        /// </summary>
        /// <remarks>Requires Windows 7 or later.</remarks>
        CanSkipForward = 0x20,
        /// <summary>
        /// The media source can skip backward in the playlist. Applies only if the <see cref="HasMultiplePresentations"/> flag is present.
        /// </summary>
        /// <remarks>Requires Windows 7 or later.</remarks>
        CanSkipBackward = 0x40,
        /// <summary>
        /// The media source is not currently using the network to receive the content. Networking hardware may enter a power saving state when this bit is set.
        /// </summary>
        /// <remarks>Requires Windows 8 or later.</remarks>
        DoesNotUseNetwork = 0x80
    }
}