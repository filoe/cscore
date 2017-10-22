using System;

namespace CSCore.DirectSound
{
    /*
     * Line 2212
     #define DSBLOCK_FROMWRITECURSOR     0x00000001
     #define DSBLOCK_ENTIREBUFFER        0x00000002
    */

    /// <summary>
    /// Defines possible flags for the <see cref="DirectSoundBuffer.Lock"/> method.
    /// </summary>
    [Flags]
    public enum DSBLock
    {
        /// <summary>
        /// The default value.
        /// </summary>
        Default = 0x00000000,
        /// <summary>
        /// Start the lock at the write cursor. The <c>offset</c> parameter is ignored.
        /// </summary>
        FromWriteCursor = 0x00000001,
        /// <summary>
        /// Lock the entire buffer. The <c>bytes</c> parameter is ignored.
        /// </summary>
        EntireBuffer = 0x00000002
    }
}