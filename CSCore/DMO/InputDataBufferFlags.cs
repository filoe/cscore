using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     Defines flags that describe an input buffer.
    ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/dd375501(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum InputDataBufferFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None,

        /// <summary>
        ///     The beginning of the data is a synchronization point.
        /// </summary>
        SyncPoint = 0x1,

        /// <summary>
        ///     The buffer's time stamp is valid. The buffer's indicated time length is valid.
        /// </summary>
        Time = 0x2,

        /// <summary>
        ///     The buffer's indicated time length is valid.
        /// </summary>
        TimeLength = 0x4
    }
}