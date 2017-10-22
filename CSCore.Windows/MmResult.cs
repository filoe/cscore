namespace CSCore
{
    //documentation taken from ftp://tellmemore.com/Diagnostic/Tools/errors_mmsystem/errors.txt
    /// <summary>
    /// Defines multi media error codes.
    /// </summary>
    public enum MmResult
    {
        /// <summary>
        /// No error.
        /// </summary>
        NoError = 0,
        /// <summary>
        /// Unspecified error.
        /// </summary>
        Error = 1,
        /// <summary>
        /// Invalid device id.
        /// </summary>
        BadDevice = 2,
        /// <summary>
        /// Driver failed enable.
        /// </summary>
        NotEnabled = 3,
        /// <summary>
        /// Device already allocated.
        /// </summary>
        Allocated = 4,
        /// <summary>
        /// Device handle is invalid.
        /// </summary>
        InvalidHandle = 5,
        /// <summary>
        /// No device driver present.
        /// </summary>
        NoDriver = 6,
        /// <summary>
        /// Memory allocation error.
        /// </summary>
        NoMemory = 7,
        /// <summary>
        /// Function isn't supported.
        /// </summary>
        NotSupported = 8,
        /// <summary>
        /// Error value out of range.
        /// </summary>
        BadErrorNumber = 9,
        /// <summary>
        /// Invalid flag passed.
        /// </summary>
        InvalidFlag = 10,
        /// <summary>
        /// Invalid parameter passed.
        /// </summary>
        InvalidParameter = 11,
        /// <summary>
        /// Handle being used simultaneously on another thread (eg callback).
        /// </summary>
        HandleBusy = 12,
        /// <summary>
        /// Specified alias not found.
        /// </summary>
        InvalidAlias = 13,
        /// <summary>
        /// Bad registry database.
        /// </summary>
        BadDatabase = 14,
        /// <summary>
        /// Registry key not found.
        /// </summary>
        KeyNotFound = 15,
        /// <summary>
        /// Registry read error.
        /// </summary>
        ReadError = 16,
        /// <summary>
        /// Registry write error.
        /// </summary>
        WriteError = 17,
        /// <summary>
        /// Registry delete error.
        /// </summary>
        DeleteError = 18,
        /// <summary>
        /// Registry value not found.
        /// </summary>
        ValueNotFound = 19,
        /// <summary>
        /// Driver does not call DriverCallback.
        /// </summary>
        NoDriverCallback = 20,
        //some header files don't include this one:
        /// <summary>
        /// More data to be returned.
        /// </summary>
        MoreData = 21,

        //waveform audio error return values:
        /// <summary>
        /// Unsupported wave format.
        /// </summary>
        BadFormat = 32,
        /// <summary>
        /// Still something playing.
        /// </summary>
        StillPlaying = 33,
        /// <summary>
        /// Header not prepared.
        /// </summary>
        Unprepared = 34,
        /// <summary>
        /// Device is synchronous.
        /// </summary>
        Synchronous = 35
    }
}