namespace CSCore.SoundOut.DirectSound
{
    public enum DSCooperativeLevelType : uint
    {
        DSSCL_NORMAL = 0x00000001,
        DSSCL_PRIORITY = 0x00000002,
        DSSCL_EXCLUSIVE = 0x00000003,
        DSSCL_WRITEPRIMARY = 0x00000004
    }
}