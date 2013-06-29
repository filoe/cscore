namespace CSCore.Codecs.FLAC
{
    public enum FlacChannelMode
    {
        NotStereo = 0,
        LeftRight = 1,
        LeftSide = 8,
        RightSide = 9,
        MidSide = 10,
        Reserved = 191
    }
}