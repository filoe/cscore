namespace CSCore.Codecs.FLAC
{
    public sealed class FlacSubFrameConstant : FlacSubFrameBase
    {
        public int Value { get; private set; }

        public FlacSubFrameConstant(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bps)
            : base(header)
        {
            Value = (int)reader.ReadBits(bps);

            unsafe
            {
                for (int i = 0; i < header.BlockSize; i++)
                {
                    int* ptr = data.destBuffer;
                    *ptr++ = Value;
                }
            }
        }
    }
}