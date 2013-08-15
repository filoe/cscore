using System;

namespace CSCore.SoundOut.DirectSound
{
    /*
     * Line 2212
     #define DSBLOCK_FROMWRITECURSOR     0x00000001
     #define DSBLOCK_ENTIREBUFFER        0x00000002
    */

    [FlagsAttribute]
    public enum DSBLock
    {
        Default = 0x00000000,
        FromWriteCursor = 0x00000001,
        EntireBuffer = 0x00000002
    }
}