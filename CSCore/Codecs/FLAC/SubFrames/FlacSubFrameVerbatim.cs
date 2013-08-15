namespace CSCore.Codecs.FLAC
{
    public sealed class FlacSubFrameVerbatim : FlacSubFrameBase
    {
        public FlacSubFrameVerbatim(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int bps)
            : base(header)
        {
            unsafe
            {
                int* ptrDest = data.destBuffer, ptrResidual = data.residualBuffer;
                int x;

                for (int i = 0; i < header.BlockSize; i++)
                {
                    x = (int)reader.ReadBits(bps);
                    *ptrDest++ = x;
                    *ptrResidual++ = x;
                }
            }
        }
    }
}