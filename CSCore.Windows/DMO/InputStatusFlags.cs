using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     InputStatusFlags.
    ///     See also: http://msdn.microsoft.com/en-us/library/windows/desktop/dd406950(v=vs.85).aspx
    /// </summary>
    [Flags]
    public enum InputStatusFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     The stream accepts data.
        /// </summary>
        AcceptData = 0x1
    }
}