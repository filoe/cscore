using System;

namespace CSCore.MediaFoundation
{
    [Flags]
    public enum MFTransformSetTypeFlags
    {
        Default = 0,

        /// <summary>
        /// Test the proposed media type, but do not set it.
        /// </summary>
        TestOnly = 1
    }
}