namespace CSCore.Codecs.FLAC
{
    public class FlacResidual
    {
        public FlacEntropyCoding CodingMethod { get; private set; }

        public int RiceOrder { get; private set; }

        internal FlacPartitionedRice Rice { get; private set; }

        public FlacResidual(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int order)
        {
            FlacEntropyCoding codingMethod = (FlacEntropyCoding)reader.ReadBits(FlacConstant.ENTROPY_CODING_METHOD_TYPE_LEN); // 2 Bit
            int riceOrder = -1;

            if (codingMethod == FlacEntropyCoding.PartitionedRice || codingMethod == FlacEntropyCoding.PartitionedRice2)
            {
                riceOrder = (int)reader.ReadBits(FlacConstant.ENTROPY_CODING_METHOD_PARTITIONED_RICE_ORDER_LEN); // 4 Bit

                FlacPartitionedRice rice = new FlacPartitionedRice(riceOrder, codingMethod, data.Content);

                if (rice.ProcessResidual(reader, header, data, order) == false)
                {
                    throw new FlacException("Decoding Flac Residual failed.", FlacLayer.SubFrame);
                }

                CodingMethod = codingMethod;
                RiceOrder = riceOrder;
                Rice = rice;
            }
            else
            {
                throw new FlacException("Not supported RICE-Coding-Method. Stream unparseable!", FlacLayer.SubFrame);
            }
        }
    }
}