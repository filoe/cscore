using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     Defines flags that specify output processing requests.
    ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/dd375511(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum ProcessOutputFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None,

        /// <summary>
        ///     Discard the output when the pointer to the output buffer is NULL.
        /// </summary>
        DiscardWhenNoBuffer = 0x1
    }
}