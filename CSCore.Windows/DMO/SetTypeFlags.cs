using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     Defines flags for setting the media type on a stream.
    ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/dd375514(v=vs.85).aspx.
    /// </summary>
    [Flags]
    public enum SetTypeFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     Test the media type but do not set it.
        /// </summary>
        TestOnly = 0x1,

        /// <summary>
        ///     Clear the media type that was set for the stream.
        /// </summary>
        Clear = 0x2
    }
}