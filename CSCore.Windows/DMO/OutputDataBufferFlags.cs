using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     Defines flags that describe an output buffer.
    ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/dd375508(v=vs.85).aspx.
    /// </summary>
    [Flags]
    public enum OutputDataBufferFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None,

        /// <summary>
        ///     The beginning of the data is a synchronization point. A synchronization point is a
        ///     random access point. For encoded video, this a sample that can be used as a decoding
        ///     start point (key frame). For uncompressed audio or video, every sample is a
        ///     synchronization point.
        /// </summary>
        SyncPoint = 0x1,

        /// <summary>
        ///     The buffer's time stamp is valid. The buffer's indicated time length is valid.
        /// </summary>
        Time = 0x2,

        /// <summary>
        ///     The buffer's indicated time length is valid.
        /// </summary>
        TimeLength = 0x4,

        /// <summary>
        ///     There is still input data available for processing, but the output buffer is full.
        /// </summary>
        Incomplete = 0x8
    }
}