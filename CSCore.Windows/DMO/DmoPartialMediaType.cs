using System;
using System.Runtime.InteropServices;

namespace CSCore.DMO
{
    /// <summary>
    ///     Describes a media type used by a Microsoft DirectX Media Object.
    /// </summary>
    /// <remarks>For more informatin, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd375510(v=vs.85).aspx"/>.</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct DmoPartialMediaType
    {
        /// <summary>
        ///     Major type GUID. Use <see cref="Guid.Empty"/> to match any major type.
        /// </summary>
        public Guid Type;

        /// <summary>
        ///     Subtype GUID. Use <see cref="Guid.Empty"/> to match any subtype.
        /// </summary>
        public Guid SubType;
    }
}