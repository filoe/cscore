using System;

namespace CSCore.DMO
{
    /// <summary>
    ///     Defines flags that specify search criteria when enumerating Microsoft DirectX Media Objects.
    ///     For more information, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd375497(v=vs.85).aspx"/>.
    /// </summary>
    /// <remarks>
    ///     A software key enables the developer of a DMO to control who uses the DMO. If a DMO has a software key,
    ///     applications must unlock the DMO to use it. The method for unlocking the DMO depends on the implementation. Consult
    ///     the documentation for the particular DMO.
    /// </remarks>
    [Flags]
    public enum DmoEnumFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None,

        /// <summary>
        ///     The enumeration should include DMOs whose use is restricted by a software key. If this flag is absent, keyed DMOs
        ///     are omitted from the enumeration.
        /// </summary>
        IncludeKeyed = 0x1
    }
}