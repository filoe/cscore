using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSCore.DMO
{
    /// <summary>
    /// Describes a media type used by a Microsoft DirectX Media Object. 
    /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd375510(v=vs.85).aspx.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DmoPartialMediaType
    {
        /// <summary>
        /// Major type GUID. Use Guid.Empty to match any major type.
        /// </summary>
        public Guid Type;
        /// <summary>
        /// Subtype GUID. Use Guid.Empty to match any subtype.
        /// </summary>
        public Guid SubType;
    }
}
