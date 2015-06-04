namespace CSCore.SoundOut.MMInterop
{
    /// <summary>
    /// uMsg
    /// http: //msdn.microsoft.com/en-us/library/dd743869%28VS.85%29.aspx
    /// </summary>
    internal enum WaveMsg
    {
        WOM_OPEN = 0x3BB,
        WOM_CLOSE = 0x3BC,
        WOM_DONE = 0x3BD,

        WIM_OPEN = 0x3BE,
        WIM_CLOSE = 0x3BF,
        WIM_DATA = 0x3C0
    }
}