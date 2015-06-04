using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    /// <summary>
    ///     Specifies the FMTID/PID identifier that programmatically identifies a property.
    /// </summary>
    /// <remarks>
    ///     For more information, see
    ///     <see href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb773381%28v=vs.85%29.aspx" />.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct PropertyKey
    {
        /// <summary>
        ///     A unique GUID for the property.
        /// </summary>
        public Guid ID;

        /// <summary>
        ///     A property identifier (PID).
        /// </summary>
        public int PropertyID;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyKey" /> struct.
        /// </summary>
        /// <param name="id">The unique GUID for the property.</param>
        /// <param name="propertyid">The property identifier (PID).</param>
        public PropertyKey(Guid id, int propertyid)
        {
            ID = id;
            PropertyID = propertyid;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ID + "/" + PropertyID;
        }
    }
}