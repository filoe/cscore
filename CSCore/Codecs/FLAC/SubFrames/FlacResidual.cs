namespace CSCore.Codecs.FLAC
{
    public class FlacResidual
    {
        public FlacEntropyCoding CodingMethod { get; private set; }

        public int RiceOrder { get; private set; }

        internal FlacPartitionedRice Rice { get; private set; }

        public FlacResidual(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int order)
        {
            FlacEntropyCoding codingMethod = (FlacEntropyCoding)reader.ReadBits(FlacConstant.EntropyCodingMethodTypeLen); // 2 Bit

            if (codingMethod == FlacEntropyCoding.PartitionedRice || codingMethod == FlacEntropyCoding.PartitionedRice2)
            {
                int riceOrder = (int)reader.ReadBits(FlacConstant.EntropyCodingMethodPartitionedRiceOrderLen);

                FlacPartitionedRice rice = new FlacPartitionedRice(riceOrder, codingMethod, data.Content);

                if (rice.ProcessResidual(reader, header, data, order) == false)
                {
                    throw new FlacException("Decoding Flac Residual failed.", FlacLayer.SubFrame);
                }

#if DEBUG
                CodingMethod = codingMethod;
                RiceOrder = riceOrder;
                Rice = rice;
#endif

            }
            else
            {
                throw new FlacException("Not supported RICE-Coding-Method. Stream unparseable!", FlacLayer.SubFrame);
            }
        }
    }
}