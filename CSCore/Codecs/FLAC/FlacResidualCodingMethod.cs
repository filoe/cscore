namespace CSCore.Codecs.FLAC
{
    internal enum FlacResidualCodingMethod
    {
        PartitionedRice = 0x0,
        PartitionedRice2 = 0x1,
        Invalid2 = 0x2, //10
        Invalid3 = 0x3 // 11
    }
}