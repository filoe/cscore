// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    internal class FlacResidual
    {
#if FLAC_DEBUG
        public FlacResidualCodingMethod CodingMethodMethod { get; private set; }

        public int PartitionOrder { get; private set; }
#endif
        public FlacResidual(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data, int order)
        {
            FlacResidualCodingMethod codingMethod = (FlacResidualCodingMethod)reader.ReadBits(2); // 2 Bit

            if (codingMethod == FlacResidualCodingMethod.PartitionedRice || codingMethod == FlacResidualCodingMethod.PartitionedRice2)
            {
                int partitionOrder = (int)reader.ReadBits(4); //"Partition order." see https://xiph.org/flac/format.html#partitioned_rice and https://xiph.org/flac/format.html#partitioned_rice2

                FlacPartitionedRice.ProcessResidual(reader, header, data, order, partitionOrder, codingMethod);

#if FLAC_DEBUG
                CodingMethodMethod = codingMethod;
                PartitionOrder = partitionOrder;
#endif

            }
            else
            {
                throw new FlacException("Not supported RICE-Coding-Method. Stream unparseable!", FlacLayer.SubFrame);
            }
        }
    }
}