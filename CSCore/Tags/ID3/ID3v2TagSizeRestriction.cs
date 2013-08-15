namespace CSCore.Tags.ID3
{
    public enum ID3v2TagSizeRestriction
    {
        Less128FramesAnd1MB = 0x0,
        Less64FramesAnd128KB = 0x40,
        Less32FramesAnd40KB = 0x80,
        Less32FramesAnd4KB = 0xC0
    }
}