using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Defines data types for <see cref="MFAttribute{TValue}"/> key/value pairs.
    /// </summary>
// ReSharper disable once InconsistentNaming
    public enum MFAttributeType
    {
        /// <summary>
        /// Unsigned 32-bit integer.
        /// </summary>
        UInt32 = 19,
        /// <summary>
        /// Unsigned 64-bit integer.
        /// </summary>
        UInt64 = 21,
        /// <summary>
        /// Floating-point number.
        /// </summary>
        Double = 5,
        /// <summary>
        /// <see cref="Guid"/> value.
        /// </summary>
        Guid = 72,
        /// <summary>
        /// Wide-character string.
        /// </summary>
        String = 31,
        /// <summary>
        /// Byte array.
        /// </summary>
        Blob = 4113,
        /// <summary>
        /// <c>IUnknown</c> pointer.
        /// </summary>
// ReSharper disable once InconsistentNaming
        IUnknown = 13
    }
}