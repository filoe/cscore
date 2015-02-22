namespace CSCore.Codecs.FLAC
{
    internal static class FlacPartitionedRice
    {
        public static unsafe void ProcessResidual(FlacBitReader reader, FlacFrameHeader header, FlacSubFrameData data,
            int order, int partitionOrder, FlacResidualCodingMethod codingMethod)
        {
            data.Content.UpdateSize(partitionOrder);
            bool isRice2 = codingMethod == FlacResidualCodingMethod.PartitionedRice2;
            int riceParameterLength = isRice2 ? 5 : 4;
            int escapeCode = isRice2 ? 31 : 15; //11111 : 1111

            int samplesPerPartition;

            int partitionCount = 1 << partitionOrder;  //2^partitionOrder -> There will be 2^order partitions. -> "order" = partitionOrder in this case

            int* residualBuffer = data.ResidualBuffer + order;

            for (int p = 0; p < partitionCount; p++)
            {
                if (partitionOrder == 0)
                    samplesPerPartition = header.BlockSize - order;
                else if (p > 0)
                    samplesPerPartition = header.BlockSize >> partitionOrder;
                else
                    samplesPerPartition = (header.BlockSize >> partitionOrder) - order;

                var riceParameter = reader.ReadBits(riceParameterLength);
                data.Content.Parameters[p] = (int)riceParameter;

                if (riceParameter >= escapeCode)
                {
                    var raw = reader.ReadBits(5); //raw is always 5 bits (see ...(+5))
                    data.Content.RawBits[p] = (int)raw;
                    for (int i = 0; i < samplesPerPartition; i++)
                    {
                        int sample = reader.ReadBitsSigned((int)raw);
                        *(residualBuffer) = sample;
                        residualBuffer++;
                    }
                }
                else
                {
                    ReadFlacRiceBlock(reader, samplesPerPartition, (int)riceParameter, residualBuffer);
                    residualBuffer += samplesPerPartition;
                }
            }
        }

        /// <summary>
        /// This method is based on the CUETools.NET BitReader (see http://sourceforge.net/p/cuetoolsnet/code/ci/default/tree/CUETools.Codecs/BitReader.cs)
        /// The author "Grigory Chudov" explicitly gave the permission to use the source as part of the cscore source code which got licensed under the ms-pl.
        /// </summary>
        private static unsafe void ReadFlacRiceBlock(FlacBitReader reader, int nvals, int riceParameter, int* ptrDest)
        {
            fixed (byte* putable = FlacBitReader.UnaryTable)
            {
                uint mask = (1u << riceParameter) - 1;
                if (riceParameter == 0)
                {
                    for (int i = 0; i < nvals; i++)
                    {
                        *(ptrDest++) = reader.ReadUnarySigned();
                    }
                }
                else
                {
                    for (int i = 0; i < nvals; i++)
                    {
                        uint bits = putable[reader.Cache >> 24];
                        uint msbs = bits;

                        while (bits == 8)
                        {
                            reader.SeekBits(8);
                            bits = putable[reader.Cache >> 24];
                            msbs += bits;
                        }

                        uint uval;
                        if (riceParameter <= 16)
                        {
                            int btsk = riceParameter + (int)bits + 1;
                            uval = (msbs << riceParameter) | ((reader.Cache >> (32 - btsk)) & mask);
                            reader.SeekBits(btsk);
                        }
                        else
                        {
                            reader.SeekBits((int)(msbs & 7) + 1);
                            uval = (msbs << riceParameter) | ((reader.Cache >> (32 - riceParameter)));
                            reader.SeekBits(riceParameter);
                        }
                        *(ptrDest++) = (int)(uval >> 1 ^ -(int)(uval & 1));
                    }
                }
            }
        }
    }
}