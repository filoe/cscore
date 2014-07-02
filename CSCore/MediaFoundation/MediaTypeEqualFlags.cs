using System;

namespace CSCore.MediaFoundation
{
    /// <summary>
    ///     Indicates the degree of similarity between the two media types.
    /// </summary>
    [Flags]
    public enum MediaTypeEqualFlags
    {
        /// <summary>
        ///     None
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     The major types are the same.
        /// </summary>
        MajorTypes = 0x1,

        /// <summary>
        ///     The subtypes are the same, or neither media type has a subtype.
        /// </summary>
        FormatTypes = 0x2,

        /// <summary>
        ///     The attributes in one of the media types are a subset of the attributes in the other, and the values of these
        ///     attributes match, excluding the value of the MF_MT_USER_DATA, MF_MT_FRAME_RATE_RANGE_MIN, and
        ///     MF_MT_FRAME_RATE_RANGE_MAX attributes.
        /// </summary>
        Data = 0x4,

        /// <summary>
        ///     The user data is identical, or neither media type contains user data. User data is specified by the MF_MT_USER_DATA
        ///     attribute.
        /// </summary>
        UserData = 0x8
    }
}