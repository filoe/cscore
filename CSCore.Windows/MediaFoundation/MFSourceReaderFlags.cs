using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    ///     Defines flags that indicate the status of the <see cref="MFSourceReader.ReadSample" /> method.
    /// </summary>
    [Flags]
    public enum MFSourceReaderFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     An error occurred. If you receive this flag, do not make any further calls to <see cref="MFSourceReader" />
        ///     methods.
        /// </summary>
        Error = 0x00000001,

        /// <summary>
        ///     The source reader reached the end of the stream.
        /// </summary>
        EndOfStream = 0x00000002,

        /// <summary>
        ///     One or more new streams were created.
        /// </summary>
        NewStream = 0x00000004,

        /// <summary>
        ///     The native format has changed for one or more streams. The native format is the format delivered by the media
        ///     source before any decoders are inserted.
        /// </summary>
        NativeMediaTypeChanged = 0x00000010,

        /// <summary>
        ///     The current media has type changed for one or more streams. To get the current media type, call the
        ///     <see cref="MFSourceReader.GetCurrentMediaType" /> method.
        /// </summary>
        CurrentMediaTypeChanged = 0x00000020,

        /// <summary>
        ///     There is a gap in the stream. This flag corresponds to an MEStreamTick event from the media source.
        /// </summary>
        StreamTick = 0x00000100,

        /// <summary>
        ///     All transforms inserted by the application have been removed for a particular stream. This could be due to a
        ///     dynamic format change from a source or decoder that prevents custom transforms from being used because they cannot
        ///     handle the new media type.
        /// </summary>
        AllEffectsRemoved = 0x00000200
    }
}